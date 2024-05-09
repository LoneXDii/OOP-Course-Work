using Client.Domain.Entitites;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Net.Http.Json;

namespace Client.Persistence.Services;

internal class ServerService : IServerService
{
    private readonly HttpClient _httpClient;
    private readonly HubConnection _chatHubConnection;

    public event Action<Message>? GetMessageFromHubEvent;

    public ServerService(HttpClient httpClient)
    {
        _httpClient = httpClient;

        _chatHubConnection = new HubConnectionBuilder().WithUrl("https://localhost:7267/chat")
                                                       .Build();
        _chatHubConnection.On<Message>("RecieveSendMessage", (message) =>
        {
            GetMessageFromHubEvent?.Invoke(message);
        });

        Task.Run(() => _chatHubConnection.StartAsync()).Wait();
    }

    public User LoginUser(string login, string password)
    {
        User? user;
        string request = $"api/Auth/authorize/login={login}&password={password}";
        user = _httpClient.GetFromJsonAsync<User>(request).Result;
        if (user is null)
        {
            throw new NullReferenceException("No such user");
        }
        return user;
    }

    public User RegisterUser(string username, string login, string password)
    {
        User? user = new User() { Name = username, Login = login, Password = password };
        string request = $"api/Auth/register";
        var response = _httpClient.PostAsJsonAsync(request, user).Result;
        user = response.Content.ReadFromJsonAsync<User>().Result;
        if (user is null)
        {
            throw new NullReferenceException("Something went wrong");
        }
        return user;
    }

    public User UpdateUser(User user) 
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
        var messages = _httpClient.GetFromJsonAsync<List<Message>>(request).Result;
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
}
