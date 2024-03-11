using Client.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Client.InfrastructureLayer.Data;

internal class ServerService
{
    private TcpClient client = new TcpClient();
    private TcpClient p2pClient = new TcpClient();
    private StreamReader? Reader = null;
    private StreamWriter? Writer = null;
    private StreamReader? p2pReader = null;
    private StreamWriter? p2pWriter = null;

    public delegate void MessageCreated(Message message);
    public event MessageCreated OnMessageCreated;

    public delegate void DialogueCreated(Dialogue dialogue);
    public event DialogueCreated OnDialogueCreated;

    public ServerService(string host, int port, int p2pPort)
    {
        client.Connect(host, port);
        Reader = new StreamReader(client.GetStream());
        Writer = new StreamWriter(client.GetStream());

        p2pClient.Connect(host, p2pPort);
        p2pReader = new StreamReader(p2pClient.GetStream());
        p2pWriter = new StreamWriter(p2pClient.GetStream());
    }

    ~ServerService()
    {
        Reader?.Close();
        Writer?.Close();
        p2pReader?.Close();
        p2pWriter?.Close();
        client.Close();
        p2pClient.Close();
    }

    public async Task<User?> CreateUserAsync(User user)
    {
        string request = JsonSerializer.Serialize(user);
        string responce = await SendRequestAsync("0001", request);
        return JsonSerializer.Deserialize<User>(responce);
    }

    public async Task<Message?> CreateMessageAsync(Message message)
    {
        string request = JsonSerializer.Serialize(message);
        string responce = await SendRequestAsync("0021", request);
        return JsonSerializer.Deserialize<Message>(responce);
    }

    public async Task<Dialogue?> CreateDialogueAsync(Dialogue dialogue)
    {
        string request = JsonSerializer.Serialize(dialogue);
        string responce = await SendRequestAsync("0011", request);
        return JsonSerializer.Deserialize<Dialogue>(responce);
    }

    public async Task<List<Dialogue>?> GetUserDialogues(int id)
    {
        string request = id.ToString();
        string responce = await SendRequestAsync("0101", request);
        return JsonSerializer.Deserialize<List<Dialogue>>(responce);
    }

    public async Task<User?> GetUserByLogin(string login)
    {
        string responce = await SendRequestAsync("1001", login);
        return JsonSerializer.Deserialize<User>(responce);
    }

    private async Task<string> SendRequestAsync(string type, string request)
    {
        string requestMessage = type + request; //type is 4 digits number
        await Writer.WriteLineAsync(requestMessage);
        await Writer.FlushAsync();
        return await GetResponceAsync();
    }

    public async Task<string> GetResponceAsync()
    {
        string? responce = await Reader.ReadLineAsync();
        if (responce == null) return "";
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

        //switch(type.ToString())
        //{
        //    case "0001":
        //        User? user = JsonSerializer.Deserialize<User>(responceMessage.ToString());
        //        OnUserCreated?.Invoke(user);
        //        break;

        //    case "0011":
        //        Dialogue? chat = JsonSerializer.Deserialize<Dialogue>(responceMessage.ToString());
        //        OnDialogueCreated?.Invoke(chat);
        //        break;

        //    case "0101":
        //        List<Dialogue>? chats = JsonSerializer.Deserialize<List<Dialogue>>(responceMessage.ToString());
        //        OnGetDialoguesList?.Invoke(chats);
        //        break;

        //    case "1001":
        //        User? OtherUser = JsonSerializer.Deserialize<User>(responceMessage.ToString());
        //        OnUserAsked?.Invoke(OtherUser);
        //        break;
        //}
        return responceMessage.ToString();
    }

    public async Task ProcessP2PConnectionAsync()
    {
        while (true)
        {
            string? responce = await p2pReader.ReadLineAsync();
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

            //switcher is temporary for test
            switch (type.ToString())
            {
                case "0021":
                    Message? message = JsonSerializer.Deserialize<Message>(responceMessage.ToString());
                    OnMessageCreated?.Invoke(message);
                    break;

                case "0011":
                    Dialogue? dialogue = JsonSerializer.Deserialize<Dialogue>(responceMessage.ToString());
                    OnDialogueCreated?.Invoke(dialogue);
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
