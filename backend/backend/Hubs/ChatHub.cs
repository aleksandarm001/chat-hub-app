using System.Collections.Concurrent;
using backend.Interfaces.Services;
using backend.Models;
using Microsoft.AspNetCore.SignalR;

namespace backend.Hubs;


public class ChatHub : Hub
{
    private static readonly ConcurrentDictionary<string, string> _userConnections = new();
    private static readonly ConcurrentDictionary<string, List<Message>> _pendingMessages = new(); 
    private readonly IMessageService _messageService;

    public ChatHub(IMessageService messageService)
    {
        _messageService = messageService;
    }
    public async Task SendPrivateMessage(long senderId, long recipientId, string messageContent)
    {
        var message = new Message(senderId, recipientId, messageContent);
        
        await _messageService.SendMessage(senderId, recipientId, messageContent);

        if (_userConnections.TryGetValue(recipientId.ToString(), out var recipientConnectionId))
        {
            await Clients.Client(recipientConnectionId).SendAsync("ReceiveMessage", senderId, message.Content);
        }
        else
        {
            if (!_pendingMessages.ContainsKey(recipientId.ToString()))
            {
                _pendingMessages[recipientId.ToString()] = new List<Message>();
            }
            _pendingMessages[recipientId.ToString()].Add(message);

            await Clients.Caller.SendAsync("UserNotAvailable", recipientId);
        }
    }

    public async Task StartChat(long userId, long targetUserId)
    {
        if (_userConnections.TryGetValue(targetUserId.ToString(), out var targetConnectionId))
        {
            await Clients.Client(targetConnectionId).SendAsync("ChatStarted", userId, targetUserId);
            await Clients.Caller.SendAsync("ChatStarted", userId, targetUserId);
        }
        else
        {
            await Clients.Caller.SendAsync("UserNotAvailable", targetUserId);
        }
    }

    public async Task RegisterUser(long userId)
    {
        _userConnections[userId.ToString()] = Context.ConnectionId;

        await Clients.Caller.SendAsync("UserRegistered", userId);

        if (_pendingMessages.ContainsKey(userId.ToString()))
        {
            var pendingMessages = _pendingMessages[userId.ToString()];
            foreach (var message in pendingMessages)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", message.SenderId, message.Content);
            }
            _pendingMessages.TryRemove(userId.ToString(), out _);
        }
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
}