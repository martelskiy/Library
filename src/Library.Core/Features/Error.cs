namespace Library.Core.Features
{
    public class Error
    {
        public Error(
            string message, 
            ErrorType type)
        {
            Message = message;
            Type = type;
        }

        public string Message { get; }
        public ErrorType Type { get; }
    }
}
