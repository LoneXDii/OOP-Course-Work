using Client.Domain.Entitites;
using Client.Persistence.Services;
using System.Collections.ObjectModel;

namespace Client.Persistence.Repositories;

public class ChatRepository
{
    private readonly IServerService _serverService;

    public ChatRepository(IServerService serverService)
    {
        _serverService = serverService;
        _serverService.UpdateChatHubEvent += UpdateFromHub;
        _serverService.GetMessageHubEvent += UpdateChatMessage;
        _serverService.UpdateMessageHubEvent += UpdateChatMessage;
        _serverService.DeleteMessageHubEvent += UpdateChatMessage;
    }

    public ObservableCollection<Chat> Chats { get; private set; } = new();

    public void GetFromServer(User user)
    {
        Chats = new ObservableCollection<Chat>(_serverService.GetUserChats(user).OrderByDescending(c => c.LastMessageDate));
    }

    public void Update(Chat chat)
    {
        _serverService.UpdateChat(chat);
    }

    public void Create(string name, User user)
    {
        Chats.Add(_serverService.CreateChat(name, user));
    }

    public Chat CreateDialogue(User user1, User user2)
    {
        return _serverService.CreateDialogue(user1, user2);
    }

    private void UpdateFromHub(Chat chat)
    {
        var tempChat = Chats.FirstOrDefault(m => m.Id == chat.Id);
        if (tempChat is null)
        {
            return;
        }
        var index = Chats.IndexOf(tempChat);
        Chats.RemoveAt(index);
        Chats.Insert(index, chat);
    }

    private void UpdateChatMessage(Message message)
    {
        try
        {
            var tempChat = Chats.FirstOrDefault(c => c.Id == message.ChatId);
            if (tempChat is null)
            {
                return;
            }
            var index = Chats.IndexOf(tempChat);
            var chat = _serverService.GetChatById(tempChat.Id);
            if (chat.LastMessageDate == tempChat.LastMessageDate && chat.LastMessage == tempChat.LastMessage)
            {
                return;
            }
            chat.Name = tempChat.Name;
            Chats.RemoveAt(index);
            tempChat = Chats.FirstOrDefault(c => c.LastMessageDate <= chat.LastMessageDate);
            if (tempChat is null)
            {
                Chats.Add(chat);
            }
            else
            {
                index = Chats.IndexOf(tempChat);
                Chats.Insert(index, chat);
            }
        }
        catch
        {
            return;
        }
    }
}
