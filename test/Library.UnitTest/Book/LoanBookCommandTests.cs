using System;
using System.CodeDom;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using Library.Core.Features;
using Library.Persistence.Features.Book;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Library.UnitTest.Book
{
    public class LoanBookCommandTests
    {
        private readonly ILoanBookCommand _sut;
        private readonly BookContext _bookContext;

        public LoanBookCommandTests()
        {
            var builder = new DbContextOptionsBuilder<BookContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
            _bookContext = new BookContext(builder.Options);

            _sut = new LoanBookCommand(
                _bookContext,
                Substitute.For<ILogger<LoanBookCommand>>());
        }

        [Fact]
        public async Task Should_ReturnNotFoundError_When_BookDoesNotExistInDatabase()
        {
            await SetupBehaviour(new BookEntity
            {
                Author = "Author",
                Name = "Name",
                Id = 1,
                IsAvailable = true
            });

            var result = await _sut.ExecuteAsync(10, CancellationToken.None);

            result.Success.ShouldBeFalse();
            result.Errors.Count().ShouldBe(1);
            result.Errors.ShouldAllBe(error => error.Type == ErrorType.NotFound && error.Message == "Book was not found");
        }

        [Fact]
        public async Task Should_SetIsAvailableToFalse_When_BookExistsInDatabase()
        {
            const int matchingBookId = 1;
            var bookEntity = new BookEntity
            {
                Author = "Author",
                Name = "Name",
                Id = matchingBookId,
                IsAvailable = true
            };

            await SetupBehaviour(bookEntity);

            var result = await _sut.ExecuteAsync(matchingBookId, CancellationToken.None);

            result.Success.ShouldBeTrue();

            var booksInDatabase = await _bookContext.Books.FirstOrDefaultAsync();
            booksInDatabase.ShouldNotBeNull();
            booksInDatabase.Author.ShouldBe(bookEntity.Author);
            booksInDatabase.Name.ShouldBe(bookEntity.Name);
            booksInDatabase.Id.ShouldBe(bookEntity.Id);
            booksInDatabase.IsAvailable.ShouldBeFalse();
        }

        private async Task SetupBehaviour(BookEntity bookEntity)
        {
            await _bookContext.Books.AddAsync(bookEntity);

            await _bookContext.SaveChangesAsync();
        }
    }
}
