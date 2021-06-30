using System;
using System.Collections.Generic;
using System.Linq;
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
    public class GetBooksQueryTests
    {
        private readonly IGetBooksQuery _sut;
        private readonly BookContext _bookContext;

        public GetBooksQueryTests()
        {
            var builder = new DbContextOptionsBuilder<BookContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
            _bookContext = new BookContext(builder.Options);

            _sut = new GetBooksQuery(
                _bookContext, 
                new BookEntityFactory(), 
                Substitute.For<ILogger<GetBooksQuery>>());
        }

        [Theory]
        [AutoData]
        public async Task Should_FetchAllBooks_When_BookAuthorIsNotSupplied(IReadOnlyList<BookEntity> booksInDatabase)
        {
            foreach (var bookEntity in booksInDatabase)
            {
                await SetupBehaviour(bookEntity);
            }

            var result = await _sut.ExecuteAsync("", CancellationToken.None);

            result.Success.ShouldBeTrue();
            var booksFromQuery = result.Data;
            booksFromQuery.Count.ShouldBe(booksInDatabase.Count);
            foreach (var bookResult in booksFromQuery)
            {
                var bookInDatabase = booksInDatabase.FirstOrDefault(entity => entity.Name == bookResult.Name);
                bookInDatabase.ShouldNotBeNull();
                bookResult.Name.ShouldBe(bookInDatabase.Name);
                bookResult.Author.ShouldBe(bookInDatabase.Author);
                bookResult.Id.ShouldBe(bookInDatabase.Id);
                bookResult.IsAvailable.ShouldBe(bookInDatabase.IsAvailable);
            }
        }

        [Theory]
        [AutoData]
        public async Task Should_FetchSpecificAuthorBook_When_AuthorIsSupplied(IReadOnlyList<BookEntity> booksInDatabase)
        {
            foreach (var bookEntity in booksInDatabase)
            {
                await SetupBehaviour(bookEntity);
            }

            var bookWithMatchingAuthor = booksInDatabase.First();

            var result = await _sut.ExecuteAsync(bookWithMatchingAuthor.Author, CancellationToken.None);

            result.Success.ShouldBeTrue();
            var booksFromQuery = result.Data;
            booksFromQuery.Count.ShouldBe(1);
            booksFromQuery.First().Author.ShouldBe(bookWithMatchingAuthor.Author);
            booksFromQuery.First().Name.ShouldBe(bookWithMatchingAuthor.Name);
            booksFromQuery.First().Id.ShouldBe(bookWithMatchingAuthor.Id);
            booksFromQuery.First().IsAvailable.ShouldBe(bookWithMatchingAuthor.IsAvailable);
        }

        private async Task SetupBehaviour(BookEntity bookEntity)
        {
            await _bookContext.Books.AddAsync(bookEntity);

            await _bookContext.SaveChangesAsync();
        }
    }
}
