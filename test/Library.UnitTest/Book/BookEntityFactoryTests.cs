using AutoFixture.Xunit2;
using Library.Persistence.Features.Book;
using Shouldly;
using Xunit;

namespace Library.UnitTest.Book
{
    public class BookEntityFactoryTests
    {
        private readonly BookEntityFactory _sut;

        public BookEntityFactoryTests()
        {
            _sut = new BookEntityFactory();
        }

        [Theory]
        [AutoData]
        public void Should_MapDomainBookCorrectly(Core.Features.Book.Book book)
        {
            var result = _sut.Create(book);
            result.Author.ShouldBe(book.Author);
            result.Name.ShouldBe(book.Name);
            result.Id.ShouldBe(default);
            result.IsAvailable.ShouldBe(book.IsAvailable);
        }

        [Theory]
        [AutoData]
        public void Should_MapBookEntityCorrectly(BookEntity book)
        {
            var result = _sut.Create(book);
            result.Author.ShouldBe(book.Author);
            result.Name.ShouldBe(book.Name);
            result.Id.ShouldBe(book.Id);
            result.IsAvailable.ShouldBe(book.IsAvailable);
        }
    }
}
