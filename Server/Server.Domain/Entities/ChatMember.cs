namespace Server.Domain.Entities;

public class ChatMember : Entity
{
    public ChatMember()
    {
        UserId = 0;
        ChatId = 0;
    }

    public ChatMember(int userId, int chatId)
    {
        UserId = userId;
        ChatId = chatId;
    }

    public int? UserId { get; set; }
    public int ChatId { get; set; }
}
