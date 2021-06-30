using System.Threading;
using System.Threading.Tasks;
using Library.Core.Features;
using Library.Persistence.Features.Book;

namespace Library.IntegrationTest.Book.Mocks
{
    public class LoanBookCommandMock : ILoanBookCommand
    {
        private readonly Result _returnResult;

        public LoanBookCommandMock(Result returnResult)
        {
            _returnResult = returnResult;
        }

        public Task<Result> ExecuteAsync(int bookId, CancellationToken cancellationToken)
        {
            return Task.FromResult(_returnResult);
        }
    }
}
