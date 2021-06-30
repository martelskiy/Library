using System.Threading.Tasks;
using FluentValidation;
using Library.Features.Book.Create;
using Shouldly;
using Xunit;

namespace Library.UnitTest.Book
{
    public class CreateBookRequestDtoValidatorTests
    {
        private readonly IValidator<CreateBookRequestDto> _sut;

        public CreateBookRequestDtoValidatorTests()
        {
            _sut = new CreateBookRequestDtoValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task Should_InvalidateInput_When_AuthorIsInvalid(string invalidAuthorInput)
        {
            var request = new CreateBookRequestDto
            {
                Author = invalidAuthorInput,
                IsAvailable = true,
                Name = "ValidName"
            };

            var result = await _sut.ValidateAsync(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.ShouldAllBe(failure => failure.ErrorMessage.Equals("'Author' must not be empty."));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task Should_InvalidateInput_When_NameIsInvalid(string invalidNameInput)
        {
            var request = new CreateBookRequestDto
            {
                Author = "ValidAuthor",
                IsAvailable = true,
                Name = invalidNameInput
            };

            var result = await _sut.ValidateAsync(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.ShouldAllBe(failure => failure.ErrorMessage.Equals("'Name' must not be empty."));
        }


        [Fact]
        public async Task Should_ValidateSuccess_When_InputIsValid()
        {
            var request = new CreateBookRequestDto
            {
                Author = "Valid author",
                IsAvailable = true,
                Name = "Valid name"
            };

            var result = await _sut.ValidateAsync(request);

            result.IsValid.ShouldBeTrue();
            result.Errors.ShouldBeEmpty();
        }
    }
}
