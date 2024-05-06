namespace Client.Domain.Entitites;

public class Message : Entity
{
    public string Text { get; set; } = "";
    public DateTime SendTime { get; set; } = DateTime.Now;
    public int? UserId { get; set; }
    public int? ChatId { get; set; }
}
