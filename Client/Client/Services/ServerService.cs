using Client.Entities.Logick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Client.Services;

internal class ServerService
{
    private TcpClient client = new TcpClient();
    private StreamReader? Reader = null;
    private StreamWriter? Writer = null;

    public delegate void UserCreated(User? user);
    public event UserCreated? OnUserCreated;

    public delegate void DialogueCreated(Dialogue? chat);
    public event DialogueCreated? OnDialogueCreated;

    public delegate void MessageCreated(Message? message);
    public event MessageCreated? OnMessageCreated;

    public delegate void GetDialoguesList(List<Dialogue>? chats);
    public event GetDialoguesList? OnGetDialoguesList;

    public delegate void GetUser(User? user);
    public event GetUser? OnUserAsked;

    public ServerService(string host, int port)
    {
        client.Connect(host, port);
        Reader = new StreamReader(client.GetStream());
        Writer = new StreamWriter(client.GetStream());
    }

    ~ServerService()
    {
        Reader?.Close();
        Writer?.Close();
    }

    public async Task CreateUserAsync(User user)
    {
        string request = JsonSerializer.Serialize(user);
        await SendRequestAsync("0001", request);
    }

    public async Task CreateMessageAsync(Message message)
    {
        string request = JsonSerializer.Serialize(message);
        await SendRequestAsync("0021", request);
    }

    public async Task CreateDialogueAsync(Dialogue dialogue)
    {
        string request = JsonSerializer.Serialize(dialogue);
        await SendRequestAsync("0011", request);
    }

    public async Task GetUserDialogues(int id)
    {
        string request = id.ToString();
        await SendRequestAsync("0101", request);
    }

    public async Task GetUserByLogin(string login)
    {
        await SendRequestAsync("1001", login);
    }

    private async Task SendRequestAsync(string type, string request)
    {
        string requestMessage = type + request; //type is 4 digits number
        await Writer.WriteLineAsync(requestMessage);
        await Writer.FlushAsync();
    }

    public async Task ProcessResponcesAsync()
    {
        while (true)
        {
            string? responce = await Reader.ReadLineAsync();
            if (responce == null) continue;
            StringBuilder type = new StringBuilder();
            StringBuilder responceMessage = new StringBuilder();
            for (int i = 0; i < 4; i++)
            {
                type.Append(responce[i]);
            }
            for (int i = 4; i < responce.Length; i++)
            {
                responceMessage.Append(responce[i]);
            }

            switch(type.ToString())
            {
                case "0001":
                    User? user = JsonSerializer.Deserialize<User>(responceMessage.ToString());
                    OnUserCreated?.Invoke(user);
                    break;

                case "0011":
                    Dialogue? chat = JsonSerializer.Deserialize<Dialogue>(responceMessage.ToString());
                    OnDialogueCreated?.Invoke(chat);
                    break;

                case "0021":
                    Message? message = JsonSerializer.Deserialize<Message>(responceMessage.ToString());
                    OnMessageCreated?.Invoke(message);
                    break;

                case "0101":
                    List<Dialogue>? chats = JsonSerializer.Deserialize<List<Dialogue>>(responceMessage.ToString());
                    OnGetDialoguesList?.Invoke(chats);
                    break;

                case "1001":
                    User? OtherUser = JsonSerializer.Deserialize<User>(responceMessage.ToString());
                    OnUserAsked?.Invoke(OtherUser);
                    break;
            }
        }
    }

}
//Types:
//0001 - create user
//0002 - authorize user
//0011 - create chat (temporary is dialogue)
//0012 - create group
//0021 - create message
//0022 - edit message
//0023 - delete message
//0101 - get user dialogues
//0102 - get dialogue messages
