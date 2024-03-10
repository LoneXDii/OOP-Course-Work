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
    private User currentUser;

    public ProgrammHandler(ServerService server)
    {
        this.server = server;
        server.OnUserCreated += CreateUser;
        server.OnChatCreated += CreateChat;
        server.OnMessageCreated += CreateMessage;
    }

    public async Task Start()
    {
        Console.WriteLine("Enter login:");
        string? login = Console.ReadLine();
        if (login != null)
        {
            await server.CreateUserAsync(new User(login));
        }
        while(true)
        {
            string? messageText = Console.ReadLine();
            if (messageText != null)
            {
                Message message = new Message(messageText);
                await server.CreateMessageAsync(message);
            }
        }
    }

    private void CreateUser(User? user)
    {
        if(user is not null) 
            currentUser = user;
    }

    private void CreateChat(IChat? chat)
    {

    }

    private void CreateMessage(Message? message)
    {
        if (message is null) return;
        chatMessages.Add(message);
        Print(message.Text);
    }

    public async Task ProcessAsync()
    {
        await server.ProcessResponcesAsync();
    }

    private void Print(string message)
    {
        if (OperatingSystem.IsWindows())
        {
            var position = Console.GetCursorPosition();
            int left = position.Left;
            int top = position.Top;

            Console.MoveBufferArea(0, top, left, 1, 0, top + 1);
            Console.SetCursorPosition(0, top);
            Console.WriteLine(message);
            Console.SetCursorPosition(left, top + 1);
        }
        else Console.WriteLine(message);
    }
}
