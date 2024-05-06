namespace Client.Persistence.Services;

internal class ServerService : IServerService
{
    private HttpClient _httpClient;

    public ServerService(HttpClient httpClient)
    {
        this._httpClient = httpClient;
    }
}
