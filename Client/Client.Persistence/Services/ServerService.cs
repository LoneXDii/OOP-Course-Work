using Client.Domain.Entitites;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;

namespace Client.Persistence.Services;

internal class ServerService : IServerService
{
    private readonly HttpClient _httpClient;
    private readonly HubConnection _chatHubConnection;
    

    public event Action<Message>? GetMessageFromHubEvent;
    public event Action<Message>? DeleteMessageFromHubEvent;
    public event Action<Message>? UpdateMessageFromHubEvent;

    public ServerService(HttpClient httpClient)
    {
        _httpClient = httpClient;

        _chatHubConnection = new HubConnectionBuilder().WithUrl("https://localhost:7267/messenger")
                                                       .Build();
        _chatHubConnection.On<Message>("SendMessage", (message) =>
        {
            GetMessageFromHubEvent?.Invoke(message);
        });

        _chatHubConnection.On<Message>("DeleteMessage", (message) =>
        {
            DeleteMessageFromHubEvent?.Invoke(message);
        });

        _chatHubConnection.On<Message>("UpdateMessage", (message) =>
        {
            UpdateMessageFromHubEvent?.Invoke(message);
        });

        Task.Run(() => _chatHubConnection.StartAsync()).Wait();
    }

    public User LoginUser(string login, string password)
    {
        User? user;
        using SHA256 hash = SHA256.Create();
        password = Convert.ToHexString(hash.ComputeHash(Encoding.UTF8.GetBytes(password)));
        string request = $"api/Auth/authorize/login={login}&password={password}";
        user = _httpClient.GetFromJsonAsync<User>(request).Result;
        if (user is null)
        {
            throw new NullReferenceException("No such user");
        }
        if (user.AuthorizationToken is null)
        {
            throw new Exception("Something went wrong");
        }
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthorizationToken);
        return user;
    }

    public User RegisterUser(string username, string login, string password)
    {
        User? user = new User() { Name = username, Login = login };
        using SHA256 hash = SHA256.Create();
        password = Convert.ToHexString(hash.ComputeHash(Encoding.UTF8.GetBytes(password)));
        user.Password = password;
        string request = $"api/Auth/register";
        var response = _httpClient.PostAsJsonAsync(request,user).Result;
        user = response.Content.ReadFromJsonAsync<User>().Result;
        if (user is null || (int)response.StatusCode != 200)
        {
            throw new Exception("Something went wrong");
        }
        if (user.AuthorizationToken == "")
        {
            throw new Exception("Something went wrong");
        }
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.AuthorizationToken);
        return user;
    }

    //Update this 
    public User UpdateUser(User user, string password) 
    {
        string request = $"api/User/update";
        var response = _httpClient.PutAsJsonAsync(request, user).Result;
        var userRes = response.Content.ReadFromJsonAsync<User>().Result;
        if (userRes is null)
        {
            throw new NullReferenceException("Something went wrong");
        }
        return userRes;
    }

    public void DeleteUser(User user)
    {
        string request = $"api/User/delete/id={user.Id}";
        var response = _httpClient.DeleteAsync(request).Result;
    }

    public List<Chat> GetUserChats(User user)
    {
        string request = $"api/User/chats/userId={user.Id}";
        var chats = _httpClient.GetFromJsonAsync<List<Chat>>(request).Result;
        if (chats is null)
        {
            throw new NullReferenceException("Something went wrong");
        }
        return chats;
    }

    public List<Message> GetChatMessages(Chat chat)
    {
        string request = $"api/Chat/getMessages/chatId={chat.Id}";
        List<Message>? messages = null;
        messages = _httpClient.GetFromJsonAsync<List<Message>>(request).Result;

        if (messages is null)
        {
            throw new NullReferenceException("Something went wrong");
        }
        return messages;
    }

    public Message SendMessage(Message message)
    {
        string request = $"api/Chat/addMessage";
        var response = _httpClient.PostAsJsonAsync(request, message).Result;
        var resMessage = response.Content.ReadFromJsonAsync<Message>().Result;
        if (resMessage is null)
        {
            throw new NullReferenceException("Something went wrong");
        }
        Task.Run(() => _chatHubConnection.InvokeAsync("SendMessage", resMessage)).Wait();
        return resMessage;
    }

    public void DeleteMessage(Message message)
    {
        string request = $"api/Chat/deleteMessage/id={message.Id}";
        var responce = _httpClient.DeleteAsync(request).Result;
        if ((int)responce.StatusCode != 200)
        {
            throw new Exception("Something went wrong");
        }
        Task.Run(() => _chatHubConnection.InvokeAsync("DeleteMessage", message)).Wait();
    }

    public void UpdateMessage(Message message)
    {
        string request = $"api/Chat/updateMessage";
        var response = _httpClient.PutAsJsonAsync(request, message).Result;
        var messageRes = response.Content.ReadFromJsonAsync<Message>().Result;
        if (messageRes is null)
        {
            throw new Exception("Something went wrong");
        }
        Task.Run(() => _chatHubConnection.InvokeAsync("UpdateMessage", message)).Wait();
    }

    public List<User> GetChatMembers(Chat chat)
    {
        string request = $"api/Chat/getMembers/chatId={chat.Id}";
        var members = _httpClient.GetFromJsonAsync<List<User>>(request).Result;
        if (members is null)
        {
            throw new Exception("Something went wrong");
        }
        return members;
    }
}
