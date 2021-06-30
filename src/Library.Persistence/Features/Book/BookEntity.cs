namespace Library.Persistence.Features.Book
{
    public class BookEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public bool IsAvailable { get; set; }
    }
}
