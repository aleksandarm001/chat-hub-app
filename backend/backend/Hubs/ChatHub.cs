using System.Collections.Concurrent;
using backend.Interfaces.Services;
using backend.Models;
using Microsoft.AspNetCore.SignalR;

namespace backend.Hubs;

public class ChatHub : Hub
{
    private static readonly ConcurrentDictionary<string, string> _userConnections = new();
    public async Task SendPrivateMessage(string senderId, string receiverId, string message)
    {
        if (_userConnections.TryGetValue(receiverId, out var receiverConnectionId))
        {
            await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", senderId, message);
        }
        else
        {
            await Clients.Caller.SendAsync("UserNotAvailable", receiverId);
        }
    }
    
    public async Task StartChat(string userId, string targetUserId)
    {
        if (_userConnections.TryGetValue(targetUserId, out var targetConnectionId))
        {
            await Clients.Client(targetConnectionId).SendAsync("ChatStarted", userId, targetUserId);
            await Clients.Caller.SendAsync("ChatStarted", userId, targetUserId);
        }
        else
        {
            await Clients.Caller.SendAsync("UserNotAvailable", targetUserId);
        }
    }
    public async Task RegisterUser(string userId)
    {
        _userConnections[userId] = Context.ConnectionId;

        await Clients.Caller.SendAsync("UserRegistered", userId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = _userConnections.FirstOrDefault(x => x.Value == Context.ConnectionId);
        if (user.Key != null)
        {
            _userConnections.TryRemove(user.Key, out _);
        }

        await base.OnDisconnectedAsync(exception);
    }
    
    
    /*
    private static readonly Dictionary<string, string> _connectedUsers = new();
    private readonly IMessageService _messageService;

    public ChatHub(IMessageService messageService)
    {
        _messageService = messageService;
    }
    public override async Task OnConnectedAsync()
    {
        try
        {
            // Retrieve user ID from query string
            var userId = Context.GetHttpContext()?.Request.Query["userId"].ToString();
        
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User ID is required to connect.");
            }

            // Add the user to the connected users dictionary
            _connectedUsers[userId] = Context.ConnectionId;

            Console.WriteLine($"User {userId} connected with Connection ID {Context.ConnectionId}");

            // Notify others about the user's status
            await SendUserStatus(userId, true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in OnConnectedAsync: {ex.Message}");
        }
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        try
        {
            // Find the user by connection ID
            var user = _connectedUsers.FirstOrDefault(x => x.Value == Context.ConnectionId);

            if (!string.IsNullOrEmpty(user.Key))
            {
                _connectedUsers.Remove(user.Key);

                Console.WriteLine($"User {user.Key} disconnected");

                // Notify others about the user's status
                await SendUserStatus(user.Key, false);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in OnDisconnectedAsync: {ex.Message}");
        }
    }
    public async Task SendPrivateMessage(string senderId, string receiverId, string message)
    {
        try
        {
            long senderIdLong = long.Parse(senderId);
            long receiverIdLong = long.Parse(receiverId);

            await _messageService.SendMessage(senderIdLong, receiverIdLong, message);

            if (_connectedUsers.TryGetValue(receiverId, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceivePrivateMessage", senderId, message);
            }
            else
            {
                throw new KeyNotFoundException("Receiver not connected.");
            }

            await Clients.Caller.SendAsync("MessageSent", receiverId);
        }
        catch (FormatException ex)
        {
            await Clients.Caller.SendAsync("Error", "Invalid user ID format.");
            Console.WriteLine($"Format error: {ex.Message}");
        }
        catch (KeyNotFoundException ex)
        {
            await Clients.Caller.SendAsync("Error", "Receiver not connected.");
            Console.WriteLine($"Receiver not found: {ex.Message}");
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", "Failed to send message.");
            Console.WriteLine($"Error in SendPrivateMessage: {ex.Message}");
        }
    }
    public async Task SendUserStatus(string user, bool isOnline)
    {
        await Clients.All.SendAsync("ReceiveUserStatus", user, isOnline);
    }
    */
}