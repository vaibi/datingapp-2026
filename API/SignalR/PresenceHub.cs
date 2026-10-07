using System;
using System.Security.Claims;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

[Authorize]
public class PresenceHub(PresenceTracker presenceTracker): Hub
{
    public override async Task OnConnectedAsync()
    {
        await presenceTracker.UserConnected(GetuserId(), Context.ConnectionId);
        await Clients.Others.SendAsync("UserOnline", GetuserId());

        var currentUsers = await presenceTracker.GetOnlineUsers();
        await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await presenceTracker.UserDisconnected(GetuserId(), Context.ConnectionId);
        await Clients.Others.SendAsync("UserOffline", GetuserId());

        var currentUsers = await presenceTracker.GetOnlineUsers();
        await Clients.All.SendAsync("GetOnlineUsers", currentUsers);

        await base.OnDisconnectedAsync(exception);
    }

    private string GetuserId()
    {
        return Context.User?.GetMemberId()
            ?? throw new HubException("cannot get member id");
    }
}