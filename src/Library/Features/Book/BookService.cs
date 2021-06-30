using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Library.Core.Features;
using Library.Persistence.Features.Book;

namespace Library.Features.Book
{
    public class BookService : IBookService
    {
        private readonly IValidator<BookDto> _validator;
        private readonly ICreateBookCommand _createBookCommand;
        private readonly BookFactory _bookFactory;

        public BookService(
            IValidator<BookDto> validator,
            ICreateBookCommand createBookCommand,
            BookFactory bookFactory)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _createBookCommand = createBookCommand ?? throw new ArgumentNullException(nameof(createBookCommand));
            _bookFactory = bookFactory ?? throw new ArgumentNullException(nameof(bookFactory));
        }

        public async Task<Result> CreateBookAsync(BookDto book, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(book, cancellationToken);

            if (!validationResult.IsValid)
            {
                return Result.Fail(validationResult.Errors.Select(failure => new Error(failure.ErrorMessage, ErrorType.Validation)).ToList());
            }
            var result = await _createBookCommand.ExecuteAsync(_bookFactory.Create(book), cancellationToken);

            return result;
        }
    }
}