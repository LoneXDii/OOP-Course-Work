namespace Server.Domain.Entities;

public class Message : Entity
{
    public Message(string text)
    {
        Text = text;
    }
    public string Text { get; set; } = "";
    public DateTime SendTime { get; set; } = DateTime.Now;
    public int? UserId { get; set; }
    public User User { get; set; }
    public int? ChatId { get; set; }
    public Chat Chat { get; set; }
}
