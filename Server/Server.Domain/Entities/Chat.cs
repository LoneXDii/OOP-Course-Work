namespace Server.Domain.Entities;

public class Chat : Entity
{
    public Chat(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
    public List<User> Users { get; set; } = new();
}
