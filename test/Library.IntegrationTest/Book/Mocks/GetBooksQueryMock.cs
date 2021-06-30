using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Library.Core.Features;
using Library.Persistence.Features.Book;

namespace Library.IntegrationTest.Book.Mocks
{
    public class GetBooksQueryMock : IGetBooksQuery
    {
        private readonly Result<IReadOnlyList<Core.Features.Book.Book>> _returnResult;
        
        public GetBooksQueryMock(Result<IReadOnlyList<Core.Features.Book.Book>> returnResult)
        {
            _returnResult = returnResult;
        }

        public Task<Result<IReadOnlyList<Core.Features.Book.Book>>> ExecuteAsync(string author, CancellationToken cancellationToken)
        {
            return Task.FromResult(_returnResult);
        }
    }
}
