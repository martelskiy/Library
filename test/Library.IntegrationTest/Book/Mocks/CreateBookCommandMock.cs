using System.Threading;
using System.Threading.Tasks;
using Library.Core.Features;
using Library.Persistence.Features.Book;

namespace Library.IntegrationTest.Book.Mocks
{
    public class CreateBookCommandMock : ICreateBookCommand
    {
        private readonly Result _returnResult;

        public CreateBookCommandMock(Result returnResult)
        {
            _returnResult = returnResult;
        }

        public Task<Result> ExecuteAsync(Core.Features.Book.Book book, CancellationToken cancellationToken)
        {
            return Task.FromResult(_returnResult);
        }
    }
}
