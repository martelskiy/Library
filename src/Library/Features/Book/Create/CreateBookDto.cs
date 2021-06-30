namespace Library.Features.Book.Create
{
    public class CreateBookDto
    {
        public string Name { get; init; }
        public string Author { get; init; }
        public bool IsAvailable { get; init; }
    }
}
