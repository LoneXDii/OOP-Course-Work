using Client.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ApplicationLayer;

internal class ApplicationService
{
    private EntitiesHandler messenger;

    public delegate void UserCreatedHandler(User user);
    public event UserCreatedHandler? OnUserCreated;

    public delegate void MessageCreatedHandler(Message message);
    public event MessageCreatedHandler? OnMessageCreated;

    public delegate void ChatCreatedHandler(Dialogue dialogue);
    public event ChatCreatedHandler? OnChatCreated;

    public ApplicationService(EntitiesHandler messenger)
    {
        this.messenger = messenger;
    }

    public void UserCreated(User user)
    {
        OnUserCreated?.Invoke(user);
    }

    public void CreateUser(User user)
    {
        messenger.CurrentUser = user;
    }

    public void MessageCreated(Message message)
    {
        OnMessageCreated?.Invoke(message);
    }

    public void AddMessage(Message message)
    {
        messenger.AddMessage(message);
    }

    public void ChatCreated(Dialogue chat)
    {
        OnChatCreated?.Invoke(chat);
    }

    public void AddChat(Dialogue dialogue)
    {
        messenger.AddDialogue(dialogue);

        //temp
        messenger.CurrentChat = dialogue;
    }
    public void AddChatUser(User user)
    {
        messenger.AddChatMember(user);
    }

    public void SetUserChats(List<Dialogue> dialogues)
    {
        messenger.Dialogues = dialogues;
    }

    public void SetChatMessages(List<Message> messages)
    {
        messenger.ChatMessages = messages;
    }
}
