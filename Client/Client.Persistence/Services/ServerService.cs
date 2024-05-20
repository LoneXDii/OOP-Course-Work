using Client.Domain.Entitites;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;

namespace Client.Persistence.Services;

internal class ServerService : IServerService
{
    private readonly HttpClient _httpClient;
    private HubConnection? _chatHubConnection;
    

    public event Action<Message>? GetMessageHubEvent;
    public event Action<Message>? DeleteMessageHubEvent;
    public event Action<Message>? UpdateMessageHubEvent;
    public event Action<User, Chat>? DeleteChatMemberHubEvent;
    public event Action<User, Chat>? AddChatMemberHubEvent;
    public event Action<Chat>? UpdateChatHubEvent;

    public ServerService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private void ConnectToHub(string token)
    {
        _chatHubConnection = new HubConnectionBuilder().WithUrl("https://localhost:7267/messenger", options =>
        {
            options.AccessTokenProvider = () => Task.FromResult(token)!;
        })
        .Build();
        _chatHubConnection.On<Message>("SendMessage", (message) =>
        {
            GetMessageHubEvent?.Invoke(message);
        });

        _chatHubConnection.On<Message>("DeleteMessage", (message) =>
        {
            DeleteMessageHubEvent?.Invoke(message);
        });

        _chatHubConnection.On<Message>("UpdateMessage", (message) =>
        {
            UpdateMessageHubEvent?.Invoke(message);
        });

        _chatHubConnection.On<User, Chat>("DeleteChatMember", (user, chat) =>
        {
            DeleteChatMemberHubEvent?.Invoke(user, chat);
        });

        _chatHubConnection.On<User, Chat>("AddChatMember", (user, chat) =>
        {
            AddChatMemberHubEvent?.Invoke(user, chat);
        });

        _chatHubConnection.On<Chat>("UpdateChat", (chat) =>
        {
            UpdateChatHubEvent?.Invoke(chat);
        });

        Task.Run(() => _chatHubConnection.StartAsync()).Wait();
    }

    private void DisconnectFromHub()
    {
        Task.Run(() => _chatHubConnection?.DisposeAsync()).Wait();
    }

    public List<User> GetAllUsers()
    {
        string request = $"api/User/allUsers";
        var users = _httpClient.GetFromJsonAsync<List<User>>(request).Result;
        if (users is null)
        {
            throw new NullReferenceException("Something went wrong");
        }
        return users;
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
        DisconnectFromHub();
        ConnectToHub(user.AuthorizationToken);
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
        DisconnectFromHub();
        ConnectToHub(user.AuthorizationToken);
        return user;
    }

    public User UpdateUsername(User user) 
    {
        string request = $"api/User/updateName";
        var response = _httpClient.PutAsJsonAsync(request, user).Result;
        var userRes = response.Content.ReadFromJsonAsync<User>().Result;
        if (userRes is null)
        {
            throw new NullReferenceException("Something went wrong");
        }
        return userRes;
    }
    
    public void UpdatePassword(User user, string oldPassword, string newPassowrd)
    {
        string request = $"api/User/updatePassword";
        using SHA256 hash = SHA256.Create();
        oldPassword = Convert.ToHexString(hash.ComputeHash(Encoding.UTF8.GetBytes(oldPassword)));
        newPassowrd = Convert.ToHexString(hash.ComputeHash(Encoding.UTF8.GetBytes(newPassowrd)));
        var requestData = new { Id = user.Id, OldPassword = oldPassword, NewPassword = newPassowrd};
        var response = _httpClient.PutAsJsonAsync(request, requestData).Result;
        if ((int)response.StatusCode != 200)
        {
            throw new Exception("Something went wrong");
        }
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

    public Message SendMessage(Message message)
    {
        string request = $"api/Chat/addMessage";
        var response = _httpClient.PostAsJsonAsync(request, message).Result;
        var resMessage = response.Content.ReadFromJsonAsync<Message>().Result;
        if (resMessage is null)
        {
            throw new NullReferenceException("Something went wrong");
        }
        Task.Run(() => _chatHubConnection?.InvokeAsync("SendMessage", resMessage)).Wait();
        return resMessage;
    }

    public void DeleteMessage(Message message)
    {
        string request = $"api/Chat/deleteMessage/id={message.Id}";
        var response = _httpClient.DeleteAsync(request).Result;
        if ((int)response.StatusCode != 200)
        {
            throw new Exception("Something went wrong");
        }
        Task.Run(() => _chatHubConnection?.InvokeAsync("DeleteMessage", message)).Wait();
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
        Task.Run(() => _chatHubConnection?.InvokeAsync("UpdateMessage", message)).Wait();
    }

    public void DeleteChatMember(Chat chat, User user)
    {
        string request = $"api/Chat/deleteUser/userId={user.Id}&chatId={chat.Id}";
        var response = _httpClient.DeleteAsync(request).Result;
        if ((int)response.StatusCode != 200)
        {
            throw new Exception("Something went wrong");
        }
        Task.Run(() => _chatHubConnection?.InvokeAsync("DeleteChatMember", user, chat)).Wait();
    }

    public void AddChatMember(Chat chat, User user)
    {
        string request = $"api/Chat/addUser";
        var requestData = new { UserId = user.Id, ChatId = chat.Id};
        var response = _httpClient.PostAsJsonAsync(request, requestData).Result;
        if ((int)response.StatusCode != 200)
        {
            throw new Exception("Something went wrong");
        }
        Task.Run(() => _chatHubConnection?.InvokeAsync("AddChatMember", user, chat)).Wait();
    }

    public void UpdateChat(Chat chat)
    {
        string request = $"api/Chat/update";
        var response = _httpClient.PutAsJsonAsync(request, chat).Result;
        var chatRes = response.Content.ReadFromJsonAsync<Chat>().Result;
        if (chatRes is null)
        {
            throw new Exception("Something went wrong");
        }
        Task.Run(() => _chatHubConnection?.InvokeAsync("UpdateChat", chatRes)).Wait();
    }

    public Chat CreateChat(string name, User creator)
    {
        string request = $"api/Chat/create";
        var requestData = new { UserId = creator.Id, Name = name };
        var response = _httpClient.PostAsJsonAsync(request, requestData).Result;
        var chat = response.Content.ReadFromJsonAsync<Chat>().Result;
        if (chat is null)
        {
            throw new Exception("Something went wrong");
        }
        return chat;
    }
}
