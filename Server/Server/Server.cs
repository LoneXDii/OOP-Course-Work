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
    private List<ClientHandler> clients = new List<ClientHandler>();
    private List<User> users = new List<User>();
    private List<Message> messages = new List<Message>();

    public async Task ListenAsync()
    {
        try
        {
            tcpListener.Start();
            Console.WriteLine("Server started\nWaiting for connection...");
            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                ClientHandler clientHandler = new ClientHandler(tcpClient, this);
                clientHandler.OnUserCreated += UserAdded;
                clientHandler.OnMessageCreated += MessageAdded;
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

    public async Task BroadcastMessageAsync(string message, string id)
    {
        foreach (var client in clients)
        {
            if(client.Id != id) {
                await client.Writer.WriteLineAsync(message);
                await client.Writer.FlushAsync();
            }
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
    }

    private async void UserAdded(User user, string id)
    {
        users.Add(user);
        user.Id = users.Count; 
        string responce = JsonSerializer.Serialize(user);
        ClientHandler? client = clients.FirstOrDefault(c => c.Id == id);
        if (client is not null)
        {
            Console.WriteLine($"Created user {user.Login} by client {client.IpAddress}");
            await client.Writer.WriteLineAsync("0001" + responce);
            await client.Writer.FlushAsync();
        }
    }

    private async void MessageAdded(Message message, string id)
    {
        ClientHandler? client = clients.FirstOrDefault(c => c.Id == id);
        messages.Add(message);
        message.Id = messages.Count;
        string responce = "0021" + JsonSerializer.Serialize(message);
        await BroadcastMessageAsync(responce, id);
        if (client is not null)
        {
            Console.WriteLine($"Message added by client {client.IpAddress}");
        }
    }
}
