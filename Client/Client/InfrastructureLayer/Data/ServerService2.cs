using Client.ApplicationLayer;
using Client.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Client.InfrastructureLayer.Data.ServerService;

namespace Client.InfrastructureLayer.Data;

internal class ServerService2
{
    private TcpClient client = new TcpClient();
    private TcpClient p2pClient = new TcpClient();
    private StreamReader? Reader = null;
    private StreamWriter? Writer = null;
    private StreamReader? p2pReader = null;
    private StreamWriter? p2pWriter = null;
    private ApplicationService application;

    public ServerService2(string host, int port, int p2pPort, ApplicationService application)
    {
        client.Connect(host, port);
        Reader = new StreamReader(client.GetStream());
        Writer = new StreamWriter(client.GetStream());

        p2pClient.Connect(host, p2pPort);
        p2pReader = new StreamReader(p2pClient.GetStream());
        p2pWriter = new StreamWriter(p2pClient.GetStream());

        this.application = application;

        this.application.OnUserCreated += CreateUserAsync;
        this.application.OnMessageCreated += CreateMessageAsync;
        this.application.OnChatCreated += CreateDialogueAsync;

        Task.Run(() => ProcessP2PConnectionAsync());
    }

    ~ServerService2()
    {
        Reader?.Close();
        Writer?.Close();
        p2pReader?.Close();
        p2pWriter?.Close();
        client.Close();
        p2pClient.Close();
    }

    public async void CreateUserAsync(User user)
    {
        string request = JsonSerializer.Serialize(user);
        string responce = await SendRequestAsync("0001", request);
        var newUser = JsonSerializer.Deserialize<User>(responce);
        if (newUser is null) return;
        application.CreateUser(newUser);
    }

    public async void CreateMessageAsync(Message message)
    {
        string request = JsonSerializer.Serialize(message);
        string responce = await SendRequestAsync("0021", request);
        var newMessage = JsonSerializer.Deserialize<Message>(responce);
        if (newMessage is null) return;
        application.AddMessage(newMessage);
    }

    public async void CreateDialogueAsync(Dialogue dialogue)
    {
        string request = JsonSerializer.Serialize(dialogue);
        string responce = await SendRequestAsync("0011", request);
        var newDialogue = JsonSerializer.Deserialize<Dialogue>(responce);
        if (newDialogue is null) return;
        application.AddChat(newDialogue);
    }

    public async Task GetUserDialogues(int id)
    {
        string request = id.ToString();
        string responce = await SendRequestAsync("0101", request);
        var dialogues = JsonSerializer.Deserialize<List<Dialogue>>(responce);
        if (dialogues is null) return;
        application.SetUserChats(dialogues);
    }

    public async void GetUserByLogin(string login)
    {
        string responce = await SendRequestAsync("1001", login);
        var user = JsonSerializer.Deserialize<User>(responce);
        if (user is null) return;
        application.AddChatUser(user);
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
                    if(message is null) return;
                    application.AddMessage(message);
                    break;

                case "0011":
                    Dialogue? dialogue = JsonSerializer.Deserialize<Dialogue>(responceMessage.ToString());
                    if(dialogue is null) return;
                    application.AddChat(dialogue);
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
