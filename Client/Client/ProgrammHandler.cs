using Client.Entities.Logick;
using Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client;

internal class ProgrammHandler
{
    private ServerService server;
    private List<IChat> chats = new List<IChat>();
    private List<User> chatMembers = new List<User>();
    private List<Message> chatMessages = new List<Message>();

    public ProgrammHandler(ServerService server)
    {
        this.server = server;
        server.OnUserCreated += CreateUser;
        server.OnChatCreated += CreateChat;
        server.OnMessageCreated += CreateMessage;
    }

    public async Task Start()
    {

    }

    private void CreateUser(User user)
    {

    }

    private void CreateChat(IChat chat)
    {

    }

    private void CreateMessage(Message message)
    {

    }

    public async Task ProcessAsync()
    {
        await server.ProcessResponcesAsync();
    }
}
