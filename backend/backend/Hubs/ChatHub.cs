using Microsoft.AspNetCore.SignalR;

namespace backend.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task SendUserStatus(string user, bool isOnline)
    {
        await Clients.All.SendAsync("ReceiveUserStatus", user, isOnline);
    }
}