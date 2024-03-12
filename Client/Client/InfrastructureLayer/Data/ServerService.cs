using Client.ApplicationLayer;
using Client.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
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
    private ApplicationService application;

    public ServerService(string host, int port, int p2pPort, ApplicationService application)
    {
        client.Connect(host, port);
        Reader = new StreamReader(client.GetStream());
        Writer = new StreamWriter(client.GetStream());

        p2pClient.Connect(host, p2pPort);
        p2pReader = new StreamReader(p2pClient.GetStream());
        p2pWriter = new StreamWriter(p2pClient.GetStream());

        this.application = application;
        this.application.OnSendRequest += SendRequestAsync;
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

    private async Task<string?> SendRequestAsync(string request)
    {
        await Writer.WriteLineAsync(request);
        await Writer.FlushAsync();
        return await GetResponceAsync();
    }

    public async Task<string?> GetResponceAsync()
    {
        string? responce = await Reader.ReadLineAsync();
        if (responce is null) return null;
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
                case "0011":
                    application.AddChat(responceMessage.ToString());
                    break;

                case "0021":
                    application.AddMessage(responceMessage.ToString());
                    break;
            }
        }
    }
}
//Types:
//0001 - create user
//0002 - authorize user

//0011 - create chat 
//0012 - add chat member
//0013 - get chat members

//0021 - create message
//0022 - edit message
//0023 - delete message
//0101 - get user dialogues
//0102 - get dialogue messages

//1001 - get user by login
