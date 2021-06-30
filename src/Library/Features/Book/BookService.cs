using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Library.Core.Features;
using Library.Features.Book.Create;
using Library.Features.Book.Fetch;
using Library.Persistence.Features.Book;

namespace Library.Features.Book
{
    public class BookService : IBookService
    {
        private readonly IValidator<CreateBookDto> _validator;
        private readonly ICreateBookCommand _createBookCommand;
        private readonly IGetBooksQuery _getBooksQuery;
        private readonly BookFactory _bookFactory;

        public BookService(
            IValidator<CreateBookDto> validator,
            ICreateBookCommand createBookCommand,
            IGetBooksQuery getBooksQuery,
            BookFactory bookFactory)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _createBookCommand = createBookCommand ?? throw new ArgumentNullException(nameof(createBookCommand));
            _getBooksQuery = getBooksQuery ?? throw new ArgumentNullException(nameof(getBooksQuery));
            _bookFactory = bookFactory ?? throw new ArgumentNullException(nameof(bookFactory));
        }

        public async Task<Result> CreateBookAsync(CreateBookDto createBook, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createBook, cancellationToken);

            if (!validationResult.IsValid)
            {
                return Result.Fail(validationResult.Errors.Select(failure => new Error(failure.ErrorMessage, ErrorType.Validation)).ToList());
            }
            var result = await _createBookCommand.ExecuteAsync(_bookFactory.Create(createBook), cancellationToken);

            return result;
        }

        public async Task<Result<IReadOnlyList<GetBookResponseDto>>> GetBooks(string author, CancellationToken cancellationToken)
        {
            var result = await _getBooksQuery.ExecuteAsync(author, cancellationToken);

            if (result.Success)
            {
                return Result<IReadOnlyList<GetBookResponseDto>>.Ok(result.Data.Select(book => _bookFactory.Create(book)).ToList());
            }

            return Result<IReadOnlyList<GetBookResponseDto>>.Fail(result.Errors);
        }
    }
}