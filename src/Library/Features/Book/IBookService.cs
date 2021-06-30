using System.Threading;
using System.Threading.Tasks;
using Library.Core.Features;

namespace Library.Features.Book
{
    public interface IBookService
    {
        public Task<Result> CreateBookAsync(BookDto book, CancellationToken cancellationToken);
    }
}