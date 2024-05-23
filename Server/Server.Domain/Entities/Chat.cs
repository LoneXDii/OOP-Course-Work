namespace Server.Domain.Entities;

public class Chat : Entity
{
    public Chat(string name)
    {
        Name = name;
    }

    public bool IsDialogue { get; set; } = false;
    public string Name { get; set; }
    public DateTime LastMessageDate { get; set; } = DateTime.Now;
    public string LastMessage { get; set; } = "";
    public List<User> Users { get; set; } = new();
}
