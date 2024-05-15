using Client.Domain.Entitites;
using Client.Persistence.Services;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Client.Persistence.Repositories;

public class MessageRepository
{
    private readonly IServerService _serverService;
    private Chat _currentChat = new();

    public MessageRepository(IServerService serverService)
    {
        _serverService = serverService;
        _serverService.GetMessageFromHubEvent += GetFromHub;
        _serverService.DeleteMessageFromHubEvent += DeleteFromHub;
    }

    public ObservableCollection<Message> Messages { get; private set; } = new();

    public void GetFromServer(Chat chat)
    {
        _currentChat = chat;
        Messages = new ObservableCollection<Message>(_serverService.GetChatMessages(chat));
    }

    public void Add(Message message)
    {
        _serverService.SendMessage(message);
    }

    public void Delete(Message message)
    {
        _serverService.DeleteMessage(message);
    }

    private void GetFromHub(Message message)
    {
        if (message.ChatId == _currentChat.Id)
        {
            Messages.Add(message);
        }
    }

    private void DeleteFromHub(Message message)
    {
        if (message.ChatId == _currentChat.Id)
        {
            var tempMessage = Messages.FirstOrDefault(m => m.Id == message.Id);
            if (tempMessage is null)
            {
                return;
            }
            Messages.Remove(tempMessage);
        }
    }
}
