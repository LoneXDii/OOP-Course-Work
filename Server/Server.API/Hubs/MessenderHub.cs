﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Server.API.DTO;
using System.Security.Claims;

namespace Server.API.Hubs;

[Authorize]
public class MessenderHub : Hub
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;
    private static Dictionary<int, string> _connections = new();

    public MessenderHub(IMediator mediator, ILogger<MessenderHub> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task SendMessage(MessageDTO message)
    {
        var clients = await GetChatConnetions((int)message.ChatId!);
        if (clients is not null)
        {
            await Clients.Clients(clients).SendAsync("SendMessage", message);
        }
    }

    public async Task DeleteMessage(MessageDTO message)
    {
        var clients = await GetChatConnetions((int)message.ChatId!);
        if (clients is not null)
        {
            await Clients.Clients(clients).SendAsync("DeleteMessage", message);
        }
    }

    public async Task UpdateMessage(MessageDTO message)
    {
        var clients = await GetChatConnetions((int)message.ChatId!);
        if (clients is not null)
        {
            await Clients.Clients(clients).SendAsync("UpdateMessage", message);
        }
    }

    public async Task DeleteChatMember(UserDTO user, ChatDTO chat)
    {
        var clients = await GetChatConnetions(chat.Id);
        if (clients is not null)
        {
            await Clients.Clients(clients).SendAsync("DeleteChatMember", user, chat);
        }
    }

    public async Task AddChatMember(UserDTO user, ChatDTO chat)
    {
        var clients = await GetChatConnetions(chat.Id);
        if (clients is not null)
        {
            try
            {
                var addedUser = _connections[user.Id];
                if (addedUser is not null)
                {
                    clients.Add(addedUser);
                }
            }
            catch{ }
            await Clients.Clients(clients).SendAsync("AddChatMember", user, chat);
        }
    }

    public async Task UpdateChat(ChatDTO chat)
    {
        var clients = await GetChatConnetions(chat.Id);
        if (clients is not null)
        {
            await Clients.Clients(clients).SendAsync("UpdateChat", chat);
        }
    }

    public override Task OnConnectedAsync()
    {
        var id = Convert.ToInt32(Context.GetHttpContext()!.User.FindFirstValue("Id"));
        var connId = Context.ConnectionId;
        try
        {
            _connections.Add(id, connId);
        }
        catch { }
        _logger.LogInformation($"Client {Context.ConnectionId} connected to hub");
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var id = Convert.ToInt32(Context.GetHttpContext()!.User.FindFirstValue("Id"));
        _connections.Remove(id);
        _logger.LogInformation($"Client {Context.ConnectionId} disconnected from hub");
        return base.OnDisconnectedAsync(exception);
    }

    private async Task<List<string>?> GetChatConnetions(int chatId)
    {
        var members = await _mediator.Send(new GetChatMembersRequest(chatId));
        if (members is null)
        {
            return null;
        }
        List<string> chatConnections = new();
        foreach (var member in members)
        {
            try
            {
                var connId = _connections[member.Id];
                chatConnections.Add(connId);
            }
            catch
            {
                continue;
            }
        }
        return chatConnections;
    }
}
