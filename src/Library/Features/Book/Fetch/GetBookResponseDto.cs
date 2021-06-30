namespace Library.Features.Book.Fetch
{
    public class GetBookResponseDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Author { get; init; }
        public bool IsAvailable { get; init; }
    }
}
