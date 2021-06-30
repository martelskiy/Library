using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Library.Core.Features;
using Library.Features.Book.Create;
using Library.Features.Book.Fetch;

namespace Library.Features.Book
{
    public interface IBookService
    {
        public Task<Result> CreateBookAsync(CreateBookDto createBook, CancellationToken cancellationToken);
        public Task<Result<IReadOnlyList<GetBookResponseDto>>> GetBooks(string author, CancellationToken cancellationToken);
    }
}