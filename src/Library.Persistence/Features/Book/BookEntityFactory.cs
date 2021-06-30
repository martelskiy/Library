namespace Library.Persistence.Features.Book
{
    public class BookEntityFactory
    {
        public BookEntity Create(Core.Features.Book.Book book)
        {
            return new()
            {
                Name = book.Name,
                Author = book.Author,
                IsAvailable = book.IsAvailable
            };
        }
    }
}
