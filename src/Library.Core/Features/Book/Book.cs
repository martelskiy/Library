namespace Library.Core.Features.Book
{
    public class Book
    {
        public Book(int id,
            string name,
            string author,
            bool isAvailable)
            :this(name, author, isAvailable)
        {
            Id = id;
        }
        public Book(
            string name,
            string author,
            bool isAvailable)
        {
            Name = name;
            Author = author;
            IsAvailable = isAvailable;
        }

        public int Id { get; }
        public string Name { get; }
        public string Author { get; }
        public bool IsAvailable { get; }
    }
}
