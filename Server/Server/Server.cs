using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Xml.Serialization;
using ServerPart.ClientEntites;
using System.Text.Json;

namespace ServerPart;

internal class Server
{
    private TcpListener tcpListener = new TcpListener(IPAddress.Any, 8888);
    private TcpListener p2pConnetion = new TcpListener(IPAddress.Any, 8080);
    private List<ClientHandler> clients = new List<ClientHandler>();

    //temporary data storage
    private List<User> users = new List<User>();
    private List<Message> messages = new List<Message>();
    private List<Chat> chats = new List<Chat>();
    private List<(User, int)> chatsMembers = new List<(User, int)>();
    public async Task ListenAsync()
    {
        try
        {
            tcpListener.Start();
            p2pConnetion.Start();
            Console.WriteLine("Server started\nWaiting for connection...");
            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                TcpClient p2pClient = await p2pConnetion.AcceptTcpClientAsync();
                ClientHandler clientHandler = new ClientHandler(tcpClient, p2pClient, this);
                clients.Add(clientHandler);
                Task.Run(clientHandler.ProcessAsync);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            Disconnect();
        }
    }

    public void RemoveConnection(string id)
    {
        ClientHandler? client = clients.FirstOrDefault(c => c.Id == id);
        if (client != null) clients.Remove(client);
        client?.Close();
    }

    private void Disconnect()
    {
        foreach(var client in clients)
        {
            client.Close();
            
        }
        tcpListener.Stop();
        p2pConnetion.Stop();
    }

    public async void UserAdded(User? user, string id)
    {
        if (user == null) return;
        users.Add(user);
        user.Id = users.Count; 
        string responce = "0001" + JsonSerializer.Serialize(user);
        ClientHandler? client = clients.FirstOrDefault(c => c.Id == id);
        if (client is not null)
        {
            Console.WriteLine($"Created user {user.Login} by client {client.IpAddress}\nSended responce {responce}");
            client.ClientUser = user;
            await client.Writer.WriteLineAsync(responce);
            await client.Writer.FlushAsync();
        }
    }

    public async void ChatAdded(Chat? chat, string id)
    {
        if (chat == null) return;

        ClientHandler? client = clients.FirstOrDefault(c => c.Id == id);
        if (client is not null && client.ClientUser is not null)
        {
            chat.Id = chats.Count;
            chats.Add(chat);
            string responce = "0011" + JsonSerializer.Serialize(chat);

            chatsMembers.Add((client.ClientUser, chat.Id));

            Console.WriteLine($"Created chat {chat.Name} by client {client.IpAddress}\nSended responce {responce}");
            await client.Writer.WriteLineAsync(responce);
            await client.Writer.FlushAsync();
        }
    }

    public async void ChatMemberAdded(string? login, int? chatId, string id)
    {
        if(login == null || chatId == null) return;
        var user = users.FirstOrDefault(c => c.Login == login);
        var chat = chats.FirstOrDefault(c => c.Id == chatId);

        if (user is null || chat is null) return;
        var client1 = clients.FirstOrDefault(c => c.Id == id);
        var client2 = clients.FirstOrDefault(c => c.ClientUser?.Id == user.Id);
        chatsMembers.Add((user, chat.Id));

        if (client1 is null || client2 is null) return;
        var responce1 = "0012" + JsonSerializer.Serialize(user);
        var responce2 = "0011" + JsonSerializer.Serialize(chat);

        Console.WriteLine($"User added to chat {chat.Id} by client {client1.IpAddress}\nSended responce {responce1}");
        await client1.Writer.WriteLineAsync(responce1);
        await client1.Writer.FlushAsync();

        Console.WriteLine($"Responce {responce2} sended to client {client2.IpAddress}");
        await client2.p2pWriter.WriteLineAsync(responce2);
        await client2.p2pWriter.FlushAsync();
    }

    public async void MessageAdded(Message? message, string id)
    {
        if(message == null) return;
        ClientHandler? client = clients.FirstOrDefault(c => c.Id == id);
        messages.Add(message);
        message.Id = messages.Count;
        string responce = "0021" + JsonSerializer.Serialize(message);
        await BroadcastMessageAsync(message, id); //redo only for chat members
        if (client is not null)
        {
            Console.WriteLine($"Message added by client {client.IpAddress} in dialogue {message.ChatId}\nSended responce {responce}");
            await client.Writer.WriteLineAsync(responce);
            await client.Writer.FlushAsync();
        }
    }

    public async Task BroadcastMessageAsync(Message message, string id)
    {
        var chatMembers = (from member in chatsMembers
                           where member.Item2 == message.ChatId
                           select member.Item1).ToList();

        var chatClients = new List<ClientHandler>();
        foreach (var member in chatMembers) 
        {
            ClientHandler? client = clients.FirstOrDefault(c => c.ClientUser?.Id == member.Id);
            if(client is not null)
                chatClients.Add(client);
        }

        string responce = "0021" + JsonSerializer.Serialize(message);

        foreach (var client in chatClients)
        {
            if (client.Id != id)
            {
                Console.WriteLine($"New message sended to client {client.IpAddress}\nSended responce {responce}");
                await client.p2pWriter.WriteLineAsync(responce);
                await client.p2pWriter.FlushAsync();
            }
        }
    }

    public async void ForChatsAsked(int userId, string id)
    {
        List<int> dialoguesIds = (from couple in chatsMembers
                                  where couple.Item1.Id == userId
                                  select couple.Item2).ToList();

        var clientChats = new List<Chat>();
        foreach (var dialogueId in dialoguesIds)
        {
            Chat? chat = chats.FirstOrDefault(c => c.Id == dialogueId);
            if(chat is not null)
                clientChats.Add(chat);
        }

        ClientHandler? client = clients.FirstOrDefault(c => c.Id == id);
        if (client is not null)
        {
            string responce = "0101" + JsonSerializer.Serialize(clientChats);
            Console.WriteLine($"Client {client.IpAddress} asked for dialogues.\nSended responce {responce}");
            await client.Writer.WriteLineAsync(responce);
            await client.Writer.FlushAsync();
        }
    }

    public async void ForUserAsked(string login, string id)
    {
        ClientHandler? client = clients.FirstOrDefault(c => c.Id == id);
        User? userAsked = null;
        foreach (var user in users)
        {
            if (user.Login == login)
            {
                userAsked = user;
                break;
            }
        }
        if (userAsked is not null && client is not null)
        {
            string responce = "1001" + JsonSerializer.Serialize(userAsked);
            Console.WriteLine($"Client {client.IpAddress} asked for user {userAsked.Id}\nSended responce {responce}");
            await client.Writer.WriteLineAsync(responce);
            await client.Writer.FlushAsync();
        }
    }

    public async void GetChatMembers(int chatId, string id)
    {
        ClientHandler? client = clients.FirstOrDefault(c => c.Id == id);
        if (client is null) return;

        var members = (from member in chatsMembers
                       where member.Item2 == chatId
                       select member.Item1).ToList();

        if (members is null) return;
        string responce = "0013"+JsonSerializer.Serialize(members);
        Console.WriteLine($"Client {client.IpAddress} asked for chat {chatId} members\nSended responce {responce}");
        await client.Writer.WriteLineAsync(responce);
        await client.Writer.FlushAsync();
    }
}
