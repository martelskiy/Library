namespace Library.Features.Book.Create
{
    public class CreateBookRequestDto
    {
        public string Name { get; init; }
        public string Author { get; init; }
        public bool IsAvailable { get; init; }
    }
}
