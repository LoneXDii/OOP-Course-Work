using Client.Domain.Entitites;
using Client.Persistence.Services;
using System.Collections.ObjectModel;

namespace Client.Persistence.Repositories;

public class ChatMembersRepository
{
    private readonly IServerService _serverService;
    private Chat _currentChat = new();

    public ChatMembersRepository(IServerService serverService)
    {
        _serverService = serverService;
        _serverService.DeleteChatMemberHubEvent += DeleteFromHub;
        _serverService.AddChatMemberHubEvent += AddFromHub;
    }

    public ObservableCollection<User> Members { get; private set; } = new();
    public Chat CurrentChat { get { return _currentChat; } }

    public void GetFromServer(Chat chat)
    {
        _currentChat = chat;
        Members = new ObservableCollection<User>(_serverService.GetChatMembers(chat));
    }

    public void Delete(User user)
    {
        _serverService.DeleteChatMember(_currentChat, user);
    }

    public void Add(User user)
    {
        _serverService.AddChatMember(_currentChat, user);
    }

    private void DeleteFromHub(User user, Chat chat)
    {
        if (chat.Id == _currentChat.Id)
        {
            var tempMemeber = Members.FirstOrDefault(m => m.Id == user.Id);
            if (tempMemeber is not null)
            {
                Members.Remove(tempMemeber);
            }
        }
    }

    private void AddFromHub(User user, Chat chat)
    {
        if (chat.Id == _currentChat.Id)
        {
            Members.Add(user);
        }
    }
}
