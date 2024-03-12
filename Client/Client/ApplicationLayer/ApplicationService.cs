using Client.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client.ApplicationLayer;

internal class ApplicationService
{
    private EntitiesHandler messenger;

    public delegate Task<string?> SendRequest(string request);
    public event SendRequest? OnSendRequest;

    public delegate void MessageAdded(string message);
    public event MessageAdded OnMessageAdded;

    public ApplicationService(EntitiesHandler messenger)
    {
        this.messenger = messenger;
    }

    public void CreateUser(string userLogin)
    {
        User? user = new User(userLogin);
        string request = "0001" + JsonSerializer.Serialize(user);
        var responce = OnSendRequest?.Invoke(request).Result;

        if (responce is null) return;
        user = JsonSerializer.Deserialize<User>(responce);

        if (user is null) return;
        messenger.CurrentUser = user;
    }

    public void CreateMessage(string messageText)
    {
        if (messenger.CurrentUser is null || messenger.CurrentChat is null) return;

        Message? message = new Message(messageText, messenger.CurrentUser.Id, messenger.CurrentChat.Id);
        string request = "0021" + JsonSerializer.Serialize(message);
        var responce = OnSendRequest?.Invoke(request).Result;

        if (responce is null) return;
        message = JsonSerializer.Deserialize<Message>(responce);

        if (message is null) return;
        messenger.AddMessage(message);
    }

    public void CreateChat(string chatName)
    {
        if(messenger.CurrentUser is null) return;

        Chat? chat = new Chat(chatName);
        string request = "0011" + JsonSerializer.Serialize(chat);
        var responce = OnSendRequest?.Invoke(request).Result;

        if (responce is null) return;
        chat = JsonSerializer.Deserialize<Chat>(responce);

        if (chat is null) return;
        messenger.AddChat(chat);
    }

    public void AddUserToChat(string userLogin)
    {
        if (messenger.CurrentUser is null || messenger.CurrentChat is null) return;

        string request = $"0012{userLogin}&{messenger.CurrentChat.Id}";
        var responce = OnSendRequest?.Invoke(request).Result;

        if (responce is null) return;
        User? user = JsonSerializer.Deserialize<User>(responce);

        if (user is null) return;
        messenger.AddChatMember(user);
    }

    public void SelectChat(int chatNum)
    {
        messenger.CurrentChat = messenger.Chats[chatNum - 1];
        _GetChatMembers(messenger.CurrentChat.Id);
    }

    private void _GetChatMembers(int chatId)
    {
        if (messenger.CurrentUser is null || messenger.CurrentChat is null) return;
        string request = $"0013{chatId}";
        var responce = OnSendRequest?.Invoke(request).Result;

        if (responce is null) return;
        var members = JsonSerializer.Deserialize<List<User>>(responce);

        if (members is null) return;
        messenger.ChatMembers = members;
    }

    public void AddChat(string chatJson)
    {
        var chat = JsonSerializer.Deserialize<Chat>(chatJson);
        if (chat is null) return;
        messenger.AddChat(chat);
    }

    public void AddMessage(string messageJson)
    {
        if (messenger.CurrentUser is null) return;

        var message = JsonSerializer.Deserialize<Message>(messageJson);
        if (message is null) return;
        messenger.AddMessage(message);

        string senderName = "";
        if (message.SenderId != messenger.CurrentUser.Id)
        {
            var sender = messenger.ChatMembers.FirstOrDefault(c => c.Id == message.SenderId);
            if (sender is not null) senderName = sender.NickName;
        
        }
        string messageText = $"{senderName}: {message.Text}";
        OnMessageAdded.Invoke(messageText);
    }

    public List<string> GetChats()
    {
        var chats = (from chat in messenger.Chats
                     select chat.Name).ToList();
        return chats;
    }
}
//Types:
//0001 - create user
//0002 - authorize user

//0011 - create chat 
//0012 - add chat member
//0013 - get chat members

//0021 - create message
//0022 - edit message
//0023 - delete message
//0101 - get user dialogues
//0102 - get dialogue messages

//1001 - get user by login