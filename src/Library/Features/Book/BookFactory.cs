namespace Library.Features.Book
{
    public class BookFactory
    {
        public Core.Features.Book.Book Create(BookDto bookDto)
        {
            return new(bookDto.Name, bookDto.Author, bookDto.IsAvailable);
        }
    }
}
