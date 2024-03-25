namespace Server.Domain.Entities;

public class Message : Entity
{
    public Message(string text, DateTime sendTime)
    {
        Text = text;
        SendTime = sendTime;
    }
    public string Text { get; set; }
    public DateTime SendTime {  get; set; }
    public int SenderId { get; set; }
    public int ChatId { get; set; }
}
