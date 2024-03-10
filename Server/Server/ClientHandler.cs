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
    private Server server;
    public string? IpAddress;

    public string Id { get; } = Guid.NewGuid().ToString();
    public int UserId { get; set; }
    public StreamWriter Writer { get; }
    public StreamReader Reader { get; }

    public delegate void UserCreated(User? user, string id);
    public event UserCreated OnUserCreated;

    public delegate void DialogueCreated(Dialogue? chat, string id);
    public event DialogueCreated OnDialogueCreated;

    public delegate void MessageCreated(Message? message, string id);
    public event MessageCreated OnMessageCreated;

    public delegate void GetDialoguesList(int userId, string id);
    public event GetDialoguesList? OnGetDialoguesList;

    public delegate void GetChatMessages(List<Message>? messages, string id);
    public event GetChatMessages? OnGetChatMessages;

    public delegate void AskForUser(string login, string id);
    public event AskForUser? OnAskForUser;

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
                            OnUserCreated.Invoke(user, Id);
                            break;

                        case "0011":
                            Dialogue? chat = JsonSerializer.Deserialize<Dialogue>(responce);
                            OnDialogueCreated.Invoke(chat, Id);
                            break;

                        case "0021":
                            Message? message = JsonSerializer.Deserialize<Message>(responce);
                            OnMessageCreated.Invoke(message, Id);
                            break;

                        case "0101":
                            int id = Convert.ToInt32(responce);
                            OnGetDialoguesList?.Invoke(id, Id);
                            break;

                        case "0102":
                            List<Message>? messages = JsonSerializer.Deserialize<List<Message>>(responce);
                            OnGetChatMessages?.Invoke(messages, Id);
                            break;

                        case "1001":
                            OnAskForUser?.Invoke(responce, Id);
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
        client.Close();
    }
}
