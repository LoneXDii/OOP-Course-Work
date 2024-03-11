using Client.Entities.Logick;
using Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client;

internal class ProgramHandler
{
    private ServerService server;
    private List<Dialogue> dialogues = new List<Dialogue>();
    private List<User> chatMembers = new List<User>();
    private List<User> users = new List<User>();
    private List<Message> chatMessages = new List<Message>();
    private User? currentUser;
    private Dialogue? currentChat;

    public delegate void MessageCreated(Message message, string senderName);
    public event MessageCreated? OnMessageCreated;

    public ProgramHandler(ServerService server)
    {
        this.server = server;
        this.server.OnMessageCreated += CreateMessage;
        this.server.OnDialogueCreated += CreateDialogue;
    }

    public async void MessagePrinted(string text)
    {
        //temporary, for tests only
        //if (currentUser is not null && currentChat is not null)
        if (currentUser is not null)
        {
            //var message = await server.CreateMessageAsync(new Message(text, currentUser.Id, currentChat.Id));
            var message = await server.CreateMessageAsync(new Message(text, currentUser.Id, 0));
            if (message is null) return;
            chatMessages.Add(message);
            string messageSender = "";
            foreach (var user in chatMembers)
            {
                if (user.Id == message.SenderId)
                {
                    messageSender = user.NickName;
                }
            }
            OnMessageCreated?.Invoke(message, messageSender);
        }
    }

    public async void Registrate(string login)
    {
        var user = await server.CreateUserAsync(new User(login));
        if (user is null) return;
        currentUser = user;

        //temp
        chatMembers.Add(user);
    }

    public async void AskUserByLogin(string login)
    {
        var user = await server.GetUserByLogin(login);
        if (user is null) return;
        users.Add(user);

        //temp
        chatMembers.Add(user);

        Dialogue? dialogue = new Dialogue(currentUser.Id, user.Id);
        dialogue = await server.CreateDialogueAsync(dialogue);
        if (dialogue is null) return;
        dialogues.Add(dialogue);
        currentChat = dialogue;
    }

    //private void CreateUser(User? user)
    //{
    //    if (user is null) return;
    //    currentUser = user;

    //    //temp
    //    chatMembers.Add(user);
    //}

    private void CreateDialogue(Dialogue? dialogue)
    {
        if (dialogue is null) return;
        dialogues.Add(dialogue);
        //temp
        currentChat = dialogue;
    }

    private void CreateMessage(Message? message)
    {
        if (message is null) return;
        chatMessages.Add(message);
        string messageSender = "";
        foreach (var user in chatMembers)
        {
            if (user.Id == message.SenderId)
            {
                messageSender = user.NickName;
            }
        }
        OnMessageCreated?.Invoke(message, messageSender);
    }

    //private async void AddToUsersList(User? user) {
    //    if(user is null) return;
    //    users.Add(user);

    //    //temp
    //    chatMembers.Add(user);

    //    Dialogue dialogue = new Dialogue(currentUser.Id, user.Id);
    //    await server.CreateDialogueAsync(dialogue);
    //}

    //public async Task ProcessAsync()
    //{
    //    await server.ProcessResponcesAsync();
    //}

    public async Task ProcessP2PAsync()
    {
        await server.ProcessP2PConnectionAsync();
    }
}
