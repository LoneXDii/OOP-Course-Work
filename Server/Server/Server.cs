using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Xml.Serialization;

namespace ServerPart;

internal class Server
{
    private TcpListener tcpListener = new TcpListener(IPAddress.Any, 8888);
    private List<ClientHandler> clients = new List<ClientHandler>();

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
}
