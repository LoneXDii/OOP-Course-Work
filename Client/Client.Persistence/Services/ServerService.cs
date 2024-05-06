using Client.Domain.Entitites;
using System.Net.Http;
using System.Net.Http.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Client.Persistence.Services;

internal class ServerService : IServerService
{
    private HttpClient _httpClient;

    public ServerService(HttpClient httpClient)
    {
        this._httpClient = httpClient;
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
        //status code checking
    }
}
