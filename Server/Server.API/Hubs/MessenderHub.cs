using Microsoft.AspNetCore.SignalR;
using Server.API.DTO;

namespace Server.API.Hubs;

public class MessenderHub : Hub
{
    public async Task SendMessage(MessageDTO message)
    {
        await Clients.All.SendAsync("SendMessage", message);
    }

    public async Task DeleteMessage(MessageDTO message)
    {
        await Clients.All.SendAsync("DeleteMessage", message);
    }

    public async Task UpdateMessage(MessageDTO message)
    {
        await Clients.All.SendAsync("UpdateMessage", message);
    }
}
