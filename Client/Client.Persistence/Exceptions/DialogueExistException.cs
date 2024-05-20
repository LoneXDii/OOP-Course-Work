namespace Client.Persistence.Exceptions;

public class DialogueExistException : Exception
{
    public DialogueExistException(string message) : base(message) { }
}
