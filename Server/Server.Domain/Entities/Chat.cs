namespace Server.Domain.Entities;

public class Chat : Entity
{
    public Chat(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
    public int? CreatorId { get; set; }
}
