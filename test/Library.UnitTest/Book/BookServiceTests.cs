using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentValidation;
using FluentValidation.Results;
using Library.Core.Features;
using Library.Features.Book;
using Library.Features.Book.Create;
using Library.Features.Book.Loan;
using Library.Persistence.Features.Book;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Library.UnitTest.Book
{
    public class BookServiceTests
    {
        private readonly IBookService _sut;
        private readonly IValidator<CreateBookRequestDto> _bookRequestValidator;
        private readonly IValidator<LoanBookRequestDto> _loanRequestValidator;
        private readonly ICreateBookCommand _createBookCommand;
        private readonly IGetBooksQuery _getBooksQuery;
        private readonly ILoanBookCommand _loanBookCommand;
        private readonly IFixture _fixture = new Fixture();

        public BookServiceTests()
        {
            _bookRequestValidator = Substitute.For<IValidator<CreateBookRequestDto>>();
            _loanRequestValidator = Substitute.For<IValidator<LoanBookRequestDto>>();
            _createBookCommand = Substitute.For<ICreateBookCommand>();
            _getBooksQuery = Substitute.For<IGetBooksQuery>();
            _loanBookCommand = Substitute.For<ILoanBookCommand>();

            _sut = new BookService(
                _bookRequestValidator, 
                _loanRequestValidator, 
                _createBookCommand, 
                _getBooksQuery,
                _loanBookCommand, 
                new BookFactory());
        }

        [Theory]
        [AutoData]
        public async Task Should_ReturnValidationFailure_When_ValidatorReturnsFail_CreateBook(CreateBookRequestDto request)
        {
            _bookRequestValidator
                .ValidateAsync(request)
                .Returns(new ValidationResult(
                    new List<ValidationFailure>
                    {
                        new("Name", "Test error message")
                    }
                ));

            var result = await _sut.CreateBookAsync(request, CancellationToken.None);
            
            result.Success.ShouldBeFalse();
            result.Errors.ShouldAllBe(error => error.Type == ErrorType.Validation);
            result.Errors.ShouldContain(error => error.Message.Contains("Test error message"));
        }

        [Theory]
        [AutoData]
        public async Task Should_NotCreateBook_When_ValidatorReturnsFail(CreateBookRequestDto request)
        {
            _bookRequestValidator
                .ValidateAsync(request)
                .Returns(new ValidationResult(
                    new List<ValidationFailure>
                    {
                        new("Name", "Test error message")
                    }
                ));

            _ = await _sut.CreateBookAsync(request, CancellationToken.None);

            await _createBookCommand
                .DidNotReceive()
                .ExecuteAsync(Arg.Any<Core.Features.Book.Book>(), Arg.Any<CancellationToken>());
        }

        [Theory]
        [AutoData]
        public async Task Should_ReturnSameResultAsCreateBookCommand_When_ValidationSucceeds(CreateBookRequestDto request)
        {
            _bookRequestValidator
                .ValidateAsync(request)
                .Returns(new ValidationResult());
            _createBookCommand
                .ExecuteAsync(Arg.Any<Core.Features.Book.Book>(), Arg.Any<CancellationToken>())
                .Returns(Result.Ok());

            var result = await _sut.CreateBookAsync(request, CancellationToken.None);

            await _createBookCommand
                .Received(1)
                .ExecuteAsync(Arg.Is<Core.Features.Book.Book>(book => book.Author == request.Author &&
                                                                      book.Name == request.Name &&
                                                                      book.IsAvailable == request.IsAvailable),
                    Arg.Any<CancellationToken>());
            result.Success.ShouldBeTrue();
        }

        [Theory]
        [AutoData]
        public async Task Should_ReturnBooks_When_QueryReturnsBooks(IReadOnlyList<Core.Features.Book.Book> booksFromQuery)
        {
            _getBooksQuery
                .ExecuteAsync("author", CancellationToken.None)
                .Returns(Result<IReadOnlyList<Core.Features.Book.Book>>.Ok(booksFromQuery));

            var result = await _sut.GetBooksAsync("author", CancellationToken.None);

            result.Success.ShouldBeTrue();
            result.Data.Count.ShouldBe(booksFromQuery.Count);
            foreach (var book in booksFromQuery)
            {
                var correspondingBookFromResult = result.Data.FirstOrDefault(dto => dto.Author == book.Author);
                correspondingBookFromResult.ShouldNotBeNull();
                correspondingBookFromResult.Author.ShouldBe(book.Author);
                correspondingBookFromResult.Name.ShouldBe(book.Name);
                correspondingBookFromResult.IsAvailable.ShouldBe(book.IsAvailable);
                correspondingBookFromResult.Id.ShouldBe(book.Id);
            }
        }

        [Fact]
        public async Task Should_ReturnFailResultWithErrorFromQuery_When_QueryReturnsErrors_GetBooks()
        {
            var errors = _fixture.CreateMany<Error>().ToList();
            _getBooksQuery
                .ExecuteAsync("author", CancellationToken.None)
                .Returns(Result<IReadOnlyList<Core.Features.Book.Book>>.Fail(errors));

            var result = await _sut.GetBooksAsync("author", CancellationToken.None);

            result.Success.ShouldBeFalse();
            result.Errors.ShouldBe(errors);
        }

        [Theory]
        [AutoData]
        public async Task Should_ReturnValidationFailure_When_ValidatorReturnsFail_LoanBook(LoanBookRequestDto request)
        {
            _loanRequestValidator
                .ValidateAsync(request)
                .Returns(new ValidationResult(
                    new List<ValidationFailure>
                    {
                        new("Name", "Test error message")
                    }
                ));

            var result = await _sut.LoanBookAsync(request, CancellationToken.None);

            result.Success.ShouldBeFalse();
            result.Errors.ShouldAllBe(error => error.Type == ErrorType.Validation);
            result.Errors.ShouldContain(error => error.Message.Contains("Test error message"));
        }

        [Theory]
        [AutoData]
        public async Task Should_NotCallLoanBookCommand_When_ValidatorReturnsFail(LoanBookRequestDto request)
        {
            _loanRequestValidator
                .ValidateAsync(request)
                .Returns(new ValidationResult(
                    new List<ValidationFailure>
                    {
                        new("Name", "Test error message")
                    }
                ));

            _ = await _sut.LoanBookAsync(request, CancellationToken.None);

            await _loanBookCommand
                .DidNotReceive()
                .ExecuteAsync(Arg.Any<int>(), Arg.Any<CancellationToken>());
        }


        [Theory]
        [AutoData]
        public async Task Should_ReturnSameResultAsLoanBookCommand_When_ValidationSucceeds(LoanBookRequestDto request)
        {
            _loanRequestValidator
                .ValidateAsync(request)
                .Returns(new ValidationResult());
            _loanBookCommand
                .ExecuteAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns(Result.Ok());

            var result = await _sut.LoanBookAsync(request, CancellationToken.None);

            await _loanBookCommand
                .Received(1)
                .ExecuteAsync(request.BookId, Arg.Any<CancellationToken>());
            result.Success.ShouldBeTrue();
        }
    }
}
