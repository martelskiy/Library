using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Library.Core.Features;

namespace Library.Persistence.Features.Book
{
    public interface IGetBooksQuery
    {
        public Task<Result<IReadOnlyList<Core.Features.Book.Book>>> ExecuteAsync(string author, CancellationToken cancellationToken);
    }
}
