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
    private TcpListener p2pConnetion = new TcpListener(IPAddress.Any, 8889);
    private List<ClientHandler> clients = new List<ClientHandler>();

    //temporary data storage
    private List<User> users = new List<User>();
    private List<Message> messages = new List<Message>();
    private List<Dialogue> dialogues = new List<Dialogue>();
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
            client.UserId = user.Id;
            await client.Writer.WriteLineAsync(responce);
            await client.Writer.FlushAsync();
        }
    }

    public async void DialogueAdded(Dialogue? dialogue, string id)
    {
        if (dialogue == null) return;
        ClientHandler? client1 = clients.FirstOrDefault(c => c.UserId == dialogue.User1Id);
        ClientHandler? client2 = clients.FirstOrDefault(c => c.UserId == dialogue.User2Id);
        dialogues.Add(dialogue);
        dialogue.Id = dialogues.Count;
        string responce = "0011" + JsonSerializer.Serialize(dialogue);
        if (client1 is not null)
        {
            Console.WriteLine($"Created dialogue {dialogue.Id} by client {client1.IpAddress}\nSended responce {responce}");
            await client1.Writer.WriteLineAsync(responce);
            await client1.Writer.FlushAsync();
        }
        if (client2 is not null)
        {
            Console.WriteLine($"Created dialogue {dialogue.Id} with client {client2.IpAddress}\nSended responce {responce}");
            await client2.p2pWriter.WriteLineAsync(responce);
            await client2.p2pWriter.FlushAsync();
        }
    }

    public async void MessageAdded(Message? message, string id)
    {
        if(message == null) return;
        ClientHandler? client = clients.FirstOrDefault(c => c.Id == id);
        messages.Add(message);
        message.Id = messages.Count;
        string responce = "0021" + JsonSerializer.Serialize(message);
        await BroadcastMessageAsync(responce, id); //redo only for chat members
        if (client is not null)
        {
            Console.WriteLine($"Message added by client {client.IpAddress} in dialogue {message.ChatId}\nSended responce {responce}");
            await client.Writer.WriteLineAsync(responce);
            await client.Writer.FlushAsync();
        }
    }

    public async Task BroadcastMessageAsync(string message, string id)
    {
        foreach (var client in clients)
        {
            if (client.Id != id)
            {
                Console.WriteLine($"New message sended to client {client.IpAddress}. Message is {message}");
                await client.p2pWriter.WriteLineAsync(message);
                await client.p2pWriter.FlushAsync();
            }
        }
    }

    public async void ForDialoguesAsked(int userId, string id)
    {
        ClientHandler? client = clients.FirstOrDefault(c => c.Id == id);
        List<Dialogue> dialogues = (from dialogue in this.dialogues
                                    where userId == dialogue.User1Id || userId == dialogue.User2Id
                                    select dialogue).ToList();
        if(client is not null)
        {
            string responce = "0101" + JsonSerializer.Serialize(dialogues);
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
}
