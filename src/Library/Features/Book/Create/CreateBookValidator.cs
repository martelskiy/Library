using FluentValidation;

namespace Library.Features.Book.Create
{
    public class CreateBookValidator : AbstractValidator<CreateBookDto>
    {
        public CreateBookValidator()
        {
            RuleFor(book => book.Author).NotEmpty();

            RuleFor(book => book.Name).NotEmpty();
        }
    }
}
