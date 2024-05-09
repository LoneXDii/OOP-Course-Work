using Microsoft.AspNetCore.SignalR;
using Server.API.DTO;

namespace Server.API.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(MessageDTO message)
    {
        await Clients.All.SendAsync("RecieveSendMessage", message);
    }
}
