namespace API.Exceptions;

public class TodoTaskNotFoundException : Exception
{
    public TodoTaskNotFoundException() { }
    public TodoTaskNotFoundException(string message) : base(message)
    {
    }
    public TodoTaskNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
