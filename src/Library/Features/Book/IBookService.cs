using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Library.Core.Features;
using Library.Features.Book.Create;
using Library.Features.Book.Fetch;
using Library.Features.Book.Loan;

namespace Library.Features.Book
{
    public interface IBookService
    {
        public Task<Result> CreateBookAsync(CreateBookRequestDto createBookRequest, CancellationToken cancellationToken);
        public Task<Result<IReadOnlyList<GetBookResponseDto>>> GetBooks(string author, CancellationToken cancellationToken);
        public Task<Result> LoanBook(LoanBookRequestDto loanBookRequestDto, CancellationToken cancellationToken);
    }
}