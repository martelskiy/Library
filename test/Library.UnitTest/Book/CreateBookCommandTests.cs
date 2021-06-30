using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Library.Persistence.Features.Book;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Library.UnitTest.Book
{
    public class CreateBookCommandTests
    {
        private readonly ICreateBookCommand _sut;
        private readonly BookContext _bookContext;

        public CreateBookCommandTests()
        {
            var builder = new DbContextOptionsBuilder<BookContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
            _bookContext = new BookContext(builder.Options);

            _sut = new CreateBookCommand(
                new BookEntityFactory(), 
                _bookContext,
                Substitute.For<ILogger<CreateBookCommand>>());
        }

        [Theory]
        [AutoData]
        public async Task Should_SaveBookIntoDatabase(Core.Features.Book.Book book)
        {
            var result = await _sut.ExecuteAsync(book, CancellationToken.None);

            result.Success.ShouldBeTrue();

            var booksInDatabase = await _bookContext.Books.FirstOrDefaultAsync();
            booksInDatabase.ShouldNotBeNull();
            booksInDatabase.IsAvailable.ShouldBe(book.IsAvailable);
            booksInDatabase.Author.ShouldBe(book.Author);
            booksInDatabase.Name.ShouldBe(book.Name);
        }
    }
}
