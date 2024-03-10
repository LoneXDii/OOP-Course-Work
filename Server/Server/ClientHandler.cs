using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerPart;

internal class ClientHandler
{
    private TcpClient client;
    private Server server;

    public string Id { get; } = Guid.NewGuid().ToString();
    public StreamWriter Writer { get; }
    public StreamReader Reader { get; }

    public ClientHandler(TcpClient client, Server server)
    {
        this.client = client;
        this.server = server;
        var stream = client.GetStream();
        Reader = new StreamReader(stream);
        Writer = new StreamWriter(stream);
    }

    public async Task ProcessAsync()
    {
        try
        {
            //Temporary (getting name and all users are in 1 group)
            string? userName = await Reader.ReadLineAsync();
            string? message = $"{userName} connected to chat. IP adress is {client.Client.RemoteEndPoint?.ToString()}";
            await server.BroadcastMessageAsync(message, Id);
            Console.WriteLine(message);
            while (true)
            {
                try
                {
                    message = await Reader.ReadLineAsync();
                    if (message == null) continue;
                    message = $"{userName}: {message}";
                    Console.WriteLine($"Message sended:\n{message}");
                    await server.BroadcastMessageAsync(message, Id);
                }
                catch 
                {
                    message = $"{userName} leaved chat";
                    Console.WriteLine($"User leaved: {userName}");
                    await server.BroadcastMessageAsync(message, Id);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            server.RemoveConnection(Id);
        }
    }

    public void Close()
    {
        Writer.Close();
        Reader.Close();
        client.Close();
    }
}
