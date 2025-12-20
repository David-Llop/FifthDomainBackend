namespace API.Exceptions;

public class TodoTaskAlreadyExistsException : Exception
{
    public TodoTaskAlreadyExistsException() { }
    public TodoTaskAlreadyExistsException(string message) : base(message) { }
    public TodoTaskAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
