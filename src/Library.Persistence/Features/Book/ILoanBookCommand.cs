using System.Threading;
using System.Threading.Tasks;
using Library.Core.Features;

namespace Library.Persistence.Features.Book
{
    public interface ILoanBookCommand
    {
        public Task<Result> ExecuteAsync(int bookId, CancellationToken cancellationToken);
    }
}
