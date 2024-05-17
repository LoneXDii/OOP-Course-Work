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

    public async Task DeleteChatMember(UserDTO user, ChatDTO chat)
    {
        await Clients.All.SendAsync("DeleteChatMember", user, chat);
    }

    public async Task UpdateChat(ChatDTO chat)
    {
        await Clients.All.SendAsync("UpdateChat", chat);
    }
}
