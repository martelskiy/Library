using System;
using System.Threading;
using System.Threading.Tasks;
using Library.Core.Features;
using Microsoft.Extensions.Logging;

namespace Library.Persistence.Features.Book
{
    public class CreateBookCommand : ICreateBookCommand
    {
        private readonly BookEntityFactory _bookEntityFactory;
        private readonly BookContext _bookContext;
        private readonly ILogger<CreateBookCommand> _logger;

        public CreateBookCommand(
            BookEntityFactory bookEntityFactory, 
            BookContext bookContext, 
            ILogger<CreateBookCommand> logger)
        {
            _bookEntityFactory = bookEntityFactory ?? throw new ArgumentNullException(nameof(bookEntityFactory));
            _bookContext = bookContext ?? throw new ArgumentNullException(nameof(bookContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result> ExecuteAsync(Core.Features.Book.Book book, CancellationToken cancellationToken)
        {
            try
            {
                var bookEntity = _bookEntityFactory.Create(book);

                await _bookContext.Books.AddAsync(bookEntity, cancellationToken);

                await _bookContext.SaveChangesAsync(cancellationToken);
                
                return Result.Ok();
            }
            catch (Exception exception)
            {
                const string errorMessage = "Failed to create a book in the database";
                _logger.LogError(exception, errorMessage);
                return Result.Fail(new []{new Error(errorMessage, ErrorType.Unspecified)});
            }
        }
    }
}