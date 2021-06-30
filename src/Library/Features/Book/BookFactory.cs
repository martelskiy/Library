using Library.Features.Book.Create;
using Library.Features.Book.Fetch;

namespace Library.Features.Book
{
    public class BookFactory
    {
        public Core.Features.Book.Book Create(CreateBookDto createBookDto)
        {
            return new(createBookDto.Name, createBookDto.Author, createBookDto.IsAvailable);
        }

        public GetBookResponseDto Create(Core.Features.Book.Book book)
        {
            return new()
            {
                Author = book.Author,
                Name = book.Name,
                IsAvailable = book.IsAvailable
            };
        }
    }
}
