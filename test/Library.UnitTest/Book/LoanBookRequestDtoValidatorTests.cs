using System.Threading.Tasks;
using FluentValidation;
using Library.Features.Book.Loan;
using Shouldly;
using Xunit;

namespace Library.UnitTest.Book
{
    public class LoanBookRequestDtoValidatorTests
    {
        private readonly IValidator<LoanBookRequestDto> _sut;

        public LoanBookRequestDtoValidatorTests()
        {
            _sut = new LoanBookRequestDtoValidator();
        }

        [Fact]
        public async Task Should_InvalidateInput_When_BookIdIsDefault()
        {
            var request = new LoanBookRequestDto
            {
                BookId = default
            };

            var result = await _sut.ValidateAsync(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.ShouldAllBe(failure => failure.ErrorMessage.Equals("'Book Id' must not be empty."));
        }

        [Fact]
        public async Task Should_ValidateSuccess_When_InputIsValid()
        {
            var request = new LoanBookRequestDto
            {
                BookId = 1
            };

            var result = await _sut.ValidateAsync(request);

            result.IsValid.ShouldBeTrue();
            result.Errors.ShouldBeEmpty();
        }
    }
}
