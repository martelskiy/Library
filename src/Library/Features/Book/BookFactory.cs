using Library.Features.Book.Create;
using Library.Features.Book.Fetch;

namespace Library.Features.Book
{
    public class BookFactory
    {
        public Core.Features.Book.Book Create(CreateBookRequestDto createBookRequestDto)
        {
            return new(createBookRequestDto.Name, createBookRequestDto.Author, createBookRequestDto.IsAvailable);
        }

        public GetBookResponseDto Create(Core.Features.Book.Book book)
        {
            return new()
            {
                Id = book.Id,
                Author = book.Author,
                Name = book.Name,
                IsAvailable = book.IsAvailable
            };
        }
    }
}
