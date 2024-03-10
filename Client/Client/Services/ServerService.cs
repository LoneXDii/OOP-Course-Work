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

    public delegate void UserCreated(User user);
    public event UserCreated OnUserCreated;

    public delegate void ChatCreated(IChat chat);
    public event ChatCreated OnChatCreated;

    public delegate void MessageCreated(Message message);
    public event MessageCreated OnMessageCreated;

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

    public async Task CreateUserAsync(string login)
    {
        await SendRequestAsync("0001", login);
    }

    public async void CreateMessageAsync(string text)
    {
        await SendRequestAsync("0021", text);
    }

    public async void CreateChatAsync()
    {
        await SendRequestAsync("0011", "");
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
                    
                    var a = JsonConvert.DeSerializeObject(responce);
                    break;

                case "0011":
                    break;

                case "0021":
                    break;
            }
        }
    }

}
//Types:
//0001 - create user
//0002 - authorize user
//0011 - create chat
//0012 - create group
//0021 - create message
//0022 - edit message
//0023 - delete message