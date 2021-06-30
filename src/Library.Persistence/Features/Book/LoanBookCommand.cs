using System;
using System.Threading;
using System.Threading.Tasks;
using Library.Core.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Library.Persistence.Features.Book
{
    public class LoanBookCommand : ILoanBookCommand
    {
        private readonly BookContext _bookContext;
        private readonly ILogger<LoanBookCommand> _logger;

        public LoanBookCommand(
            BookContext bookContext, 
            ILogger<LoanBookCommand> logger)
        {
            _bookContext = bookContext ?? throw new ArgumentNullException(nameof(bookContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result> ExecuteAsync(int bookId, CancellationToken cancellationToken)
        {
            try
            {
                var bookEntity = await _bookContext
                    .Books
                    .FirstOrDefaultAsync(entity => entity.Id.Equals(bookId), cancellationToken);

                if (bookEntity == null)
                {
                    return Result.Fail(new []{new Error("Book was not found", ErrorType.NotFound)});
                }

                bookEntity.IsAvailable = false;

                await _bookContext.SaveChangesAsync(cancellationToken);

                return Result.Ok();
            }
            catch (Exception exception)
            {
                const string errorMessage = "Failed to upload book entity";
                _logger.LogError(exception, errorMessage);
                return Result.Fail(new[] { new Error(errorMessage, ErrorType.Unspecified) });
            }
        }
    }
}