namespace Client.Domain.Entitites;

public class Chat : Entity
{
    public string Name { get; set; } = "";
    public bool IsDialogue { get; set; }
}
