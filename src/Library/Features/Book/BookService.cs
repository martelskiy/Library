using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Library.Core.Features;
using Library.Features.Book.Create;
using Library.Features.Book.Fetch;
using Library.Features.Book.Loan;
using Library.Persistence.Features.Book;

namespace Library.Features.Book
{
    public class BookService : IBookService
    {
        private readonly IValidator<CreateBookRequestDto> _createBookRequestValidator;
        private readonly IValidator<LoanBookRequestDto> _loanRequestValidator;
        private readonly ICreateBookCommand _createBookCommand;
        private readonly IGetBooksQuery _getBooksQuery;
        private readonly ILoanBookCommand _loanBookCommand;
        private readonly BookFactory _bookFactory;

        public BookService(
            IValidator<CreateBookRequestDto> createBookRequestValidator,
            IValidator<LoanBookRequestDto> loanRequestValidator,
            ICreateBookCommand createBookCommand,
            IGetBooksQuery getBooksQuery,
            ILoanBookCommand loanBookCommand,
            BookFactory bookFactory)
        {
            _createBookRequestValidator = createBookRequestValidator ?? throw new ArgumentNullException(nameof(createBookRequestValidator));
            _loanRequestValidator = loanRequestValidator ?? throw new ArgumentNullException(nameof(loanRequestValidator));
            _createBookCommand = createBookCommand ?? throw new ArgumentNullException(nameof(createBookCommand));
            _getBooksQuery = getBooksQuery ?? throw new ArgumentNullException(nameof(getBooksQuery));
            _loanBookCommand = loanBookCommand ?? throw new ArgumentNullException(nameof(loanBookCommand));
            _bookFactory = bookFactory ?? throw new ArgumentNullException(nameof(bookFactory));
        }

        public async Task<Result> CreateBookAsync(CreateBookRequestDto createBookRequest, CancellationToken cancellationToken)
        {
            var validationResult = await _createBookRequestValidator.ValidateAsync(createBookRequest, cancellationToken);

            if (!validationResult.IsValid)
            {
                return Result.Fail(validationResult.Errors.Select(failure => new Error(failure.ErrorMessage, ErrorType.Validation)).ToList());
            }
            var result = await _createBookCommand.ExecuteAsync(_bookFactory.Create(createBookRequest), cancellationToken);

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

        public async Task<Result> LoanBook(LoanBookRequestDto loanBookRequestDto, CancellationToken cancellationToken)
        {
            var validationResult = await _loanRequestValidator.ValidateAsync(loanBookRequestDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                return Result.Fail(validationResult.Errors.Select(failure => new Error(failure.ErrorMessage, ErrorType.Validation)).ToList());
            }

            var result = await _loanBookCommand.ExecuteAsync(loanBookRequestDto.BookId, cancellationToken);

            return result;
        }
    }
}