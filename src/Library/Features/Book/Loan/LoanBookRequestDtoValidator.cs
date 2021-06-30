using FluentValidation;

namespace Library.Features.Book.Loan
{
    public class LoanBookRequestDtoValidator : AbstractValidator<LoanBookRequestDto>
    {
        public LoanBookRequestDtoValidator()
        {
            RuleFor(dto => dto.BookId).NotEmpty();
        }
    }
}
