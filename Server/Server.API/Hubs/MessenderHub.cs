using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Server.API.DTO;
using System.Security.Claims;

namespace Server.API.Hubs;

[Authorize]
public class MessenderHub : Hub
{
    private readonly IMediator _mediator;
    private Dictionary<int, string> _connections = new();
    //add dictionary with userId and connectionId

    public MessenderHub(IMediator mediator)
    {
        _mediator = mediator;
    }

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

    public async Task AddChatMember(UserDTO user, ChatDTO chat)
    {
        await Clients.All.SendAsync("AddChatMember", user, chat);
    }

    public async Task UpdateChat(ChatDTO chat)
    {
        await Clients.All.SendAsync("UpdateChat", chat);
    }

    public override Task OnConnectedAsync()
    {
        var id = Convert.ToInt32(Context.GetHttpContext()!.User.FindFirstValue("Id"));
        var connId = Context.ConnectionId;
        _connections.Add(id, connId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var id = Convert.ToInt32(Context.GetHttpContext()!.User.FindFirstValue("Id"));
        _connections.Remove(id);
        return base.OnDisconnectedAsync(exception);
    }
}
