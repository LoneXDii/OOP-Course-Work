namespace Server.API.DTO;

public class MessageDTO
{
    public int Id { get; set; }
    public string Text { get; set; } = "";
    public DateTime SendTime { get; set; } = DateTime.Now;
    public string UserName { get; set; } = "";
    public int? UserId { get; set; }
    public int? ChatId { get; set; }
}
