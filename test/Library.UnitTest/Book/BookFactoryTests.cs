using AutoFixture.Xunit2;
using Library.Features.Book;
using Library.Features.Book.Create;
using Shouldly;
using Xunit;

namespace Library.UnitTest.Book
{
    public class BookFactoryTests
    {
        private readonly BookFactory _sut;

        public BookFactoryTests()
        {
            _sut = new BookFactory();
        }

        [Theory]
        [AutoData]
        public void Should_MapDomainBookCorrectly(Core.Features.Book.Book book)
        {
            var result = _sut.Create(book);
            result.Author.ShouldBe(book.Author);
            result.Name.ShouldBe(book.Name);
            result.Id.ShouldBe(book.Id);
            result.IsAvailable.ShouldBe(book.IsAvailable);
        }

        [Theory]
        [AutoData]
        public void Should_MapDomainBookRequstDtoCorrectly(CreateBookRequestDto book)
        {
            var result = _sut.Create(book);
            result.Author.ShouldBe(book.Author);
            result.Name.ShouldBe(book.Name);
            result.IsAvailable.ShouldBe(book.IsAvailable);
        }
    }
}
