using System.Threading;
using System.Threading.Tasks;
using Library.Core.Features;

namespace Library.Persistence.Features.Book
{
    public interface ICreateBookCommand
    {
        Task<Result> ExecuteAsync(Core.Features.Book.Book book, CancellationToken cancellationToken);
    }
}
