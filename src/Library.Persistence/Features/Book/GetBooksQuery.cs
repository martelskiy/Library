using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Library.Core.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Library.Persistence.Features.Book
{
    public class GetBooksQuery : IGetBooksQuery
    {
        private readonly BookContext _bookContext;
        private readonly BookEntityFactory _bookEntityFactory;
        private readonly ILogger<GetBooksQuery> _logger;

        public GetBooksQuery(
            BookContext bookContext, 
            BookEntityFactory bookEntityFactory, 
            ILogger<GetBooksQuery> logger)
        {
            _bookContext = bookContext ?? throw new ArgumentNullException(nameof(bookContext));
            _bookEntityFactory = bookEntityFactory ?? throw new ArgumentNullException(nameof(bookEntityFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<IReadOnlyList<Core.Features.Book.Book>>> ExecuteAsync(string author, CancellationToken cancellationToken)
        {
            try
            {
                List<BookEntity> books;

                if (!string.IsNullOrWhiteSpace(author))
                {
                    books = await _bookContext
                        .Books
                        .Where(entity => entity.Author.Equals(author, StringComparison.InvariantCultureIgnoreCase))
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
                }
                else
                {
                    books = await _bookContext
                        .Books
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
                }

                return Result<IReadOnlyList<Core.Features.Book.Book>>.Ok(books.Select(entity => _bookEntityFactory.Create(entity)).ToList());
            }
            catch (Exception exception)
            {
                const string errorMessage = "Failed to fetch books from the database";
                _logger.LogError(exception, errorMessage);
                return Result<IReadOnlyList<Core.Features.Book.Book>>.Fail(new[] { new Error(errorMessage, ErrorType.Unspecified) });
            }
        }
    }
}