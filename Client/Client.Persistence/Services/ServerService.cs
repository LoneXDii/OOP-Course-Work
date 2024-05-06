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

    public User Login(string login, string password)
    {
        User? user;
        string request = $"api/Auth/authorize/login={login}&password={password}";
        user = Task.Run(async () => await _httpClient.GetFromJsonAsync<User>(request)).Result;
        if (user is null)
        {
            throw new NullReferenceException("No such user");
        }
        return user;
    }
}
