namespace Server.API.DTO;

public class ChatDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public DateTime LastMessageDate { get; set; } = DateTime.Now;
    public string LastMessage { get; set; } = "";
    public bool IsDialogue { get; set; }
}
