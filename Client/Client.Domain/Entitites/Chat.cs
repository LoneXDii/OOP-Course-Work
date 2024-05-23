namespace Client.Domain.Entitites;

public class Chat : Entity
{
    public string Name { get; set; } = "";
    public DateTime LastMessageDate { get; set; } = DateTime.Now;
    public string LastMessage { get; set; } = "";
    public bool IsDialogue { get; set; }
}
