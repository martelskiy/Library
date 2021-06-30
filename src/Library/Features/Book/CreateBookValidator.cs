using FluentValidation;

namespace Library.Features.Book
{
    public class CreateBookValidator : AbstractValidator<BookDto>
    {
        public CreateBookValidator()
        {
            RuleFor(book => book.Author).NotEmpty();

            RuleFor(book => book.Name).NotEmpty();
        }
    }
}
