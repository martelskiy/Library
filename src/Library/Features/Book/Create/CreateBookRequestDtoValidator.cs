using FluentValidation;

namespace Library.Features.Book.Create
{
    public class CreateBookRequestDtoValidator : AbstractValidator<CreateBookRequestDto>
    {
        public CreateBookRequestDtoValidator()
        {
            RuleFor(book => book.Author).NotEmpty();

            RuleFor(book => book.Name).NotEmpty();
        }
    }
}
