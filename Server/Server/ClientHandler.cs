using ServerPart.ClientEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerPart;

internal class ClientHandler
{
    private TcpClient client;
    private TcpClient p2pClient;
    private Server server;
    public string? IpAddress;

    public string Id { get; } = Guid.NewGuid().ToString();
    public int UserId { get; set; }
    public StreamWriter Writer { get; }
    public StreamReader Reader { get; }
    public StreamWriter p2pWriter { get; }
    public StreamReader p2pReader { get; }

    public ClientHandler(TcpClient client, TcpClient p2pClient, Server server)
    {
        this.client = client;
        this.server = server;
        this.p2pClient = p2pClient;
        var stream = client.GetStream();
        var p2pStream = p2pClient.GetStream();
        Reader = new StreamReader(stream);
        Writer = new StreamWriter(stream);
        p2pReader = new StreamReader(p2pStream);
        p2pWriter = new StreamWriter(p2pStream);
    }

    public async Task ProcessAsync()
    {
        try
        {
            IpAddress = client.Client.RemoteEndPoint?.ToString();
            string? serverMessage = $"Client is connected. IP adress is {IpAddress}";
            Console.WriteLine(serverMessage);
            while (true)
            {
                try
                {
                    string? responce = await Reader.ReadLineAsync();
                    if (responce == null) continue;
                    StringBuilder type = new StringBuilder();
                    for (int i = 0; i < 4; i++)
                    {
                        type.Append(responce[i]);
                    }
                    responce = responce.Remove(0,4);

                    switch (type.ToString())
                    {
                        case "0001":
                            User? user = JsonSerializer.Deserialize<User>(responce);
                            server.UserAdded(user, Id);
                            break;

                        case "0011":
                            Dialogue? chat = JsonSerializer.Deserialize<Dialogue>(responce);
                            server.DialogueAdded(chat, Id);
                            break;

                        case "0021":
                            Message? message = JsonSerializer.Deserialize<Message>(responce);
                            server.MessageAdded(message, Id);
                            break;

                        case "0101":
                            int id = Convert.ToInt32(responce);
                            server.ForDialoguesAsked(id, Id);
                            break;

                        //case "0102":
                        //    List<Message>? messages = JsonSerializer.Deserialize<List<Message>>(responce);
                        //    OnGetChatMessages?.Invoke(messages, Id);
                        //    break;

                        case "1001":
                            server.ForUserAsked(responce, Id);
                            break;
                    }
                }
                catch 
                {
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
            string? serverMessage = $"Client is disconnected. IP adress is {IpAddress}";
            Console.WriteLine(serverMessage);
            server.RemoveConnection(Id);
        }
    }

    public void Close()
    {
        Writer.Close();
        Reader.Close();
        p2pWriter.Close();
        p2pReader.Close();
        client.Close();
    }
}
