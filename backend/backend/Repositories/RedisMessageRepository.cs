using backend.Extensions;
using backend.Interfaces.Repositories;
using backend.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace backend.Repositories;

public class RedisMessageRepository : IMessageRepository
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _database;

    public RedisMessageRepository(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _database = redis.GetDatabase();
    }
    
    public async Task CreateChat(long userId1, long userId2)
    {
        try
        {
            var chat = new Chat(userId1, userId2);
            var chatKey = GetChatKey(userId1, userId2);
            var messagesKey = $"{chatKey}:messages";
            chat.Messages = messagesKey;
            var hashEntries = RedisHelper.ToHashEntries(chat);

            await _database.HashSetAsync(chatKey, hashEntries);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in CreateChat: {ex.Message}");
            throw new Exception("An error occurred while creating the chat.");
        }
    }

    public async Task CreateMessage(long userId1, long userId2, string content)
    {
        try
        {
            var chatKey = GetChatKey(userId1, userId2);
            var messagesKey = await _database.HashGetAsync(chatKey, "Messages");

            if (messagesKey.IsNullOrEmpty)
            {
                throw new Exception("Chat not found or messages key missing.");
            }

            var message = new Message(userId1, userId2, content);
            var messageJson = JsonConvert.SerializeObject(message);
            await _database.SortedSetAddAsync(messagesKey.ToString(), messageJson, message.Timestamp.ToOADate());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AddMessage: {ex.Message}");
            throw new Exception("An error occurred while adding the message.");
        }
    }
    
    public async Task<List<Message>> GetMessages(long userId1, long userId2)
    {
        try
        {
            var messagesKey = $"{GetChatKey(userId1, userId2)}:messages";

            var messageEntries = await _database.SortedSetRangeByScoreAsync(messagesKey);

            var messages = messageEntries
                .Select(entry => JsonConvert.DeserializeObject<Message>(entry))
                .ToList();

            return messages;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetMessages: {ex.Message}");
            throw new Exception("An error occurred while retrieving messages.");
        }
    }

    public async Task ReadMessage(long userId1, long userId2)
    {
        try
        {
            var chatKey = GetChatKey(userId1, userId2);
            var messagesKey = $"{chatKey}:messages";

            var lastReadTimestamp = await GetLastReadTimeStamp(userId1, chatKey);
            
            double lastReadScore = double.TryParse(lastReadTimestamp, out var result) ? result : 0.0;

            var newMessages = await _database.SortedSetRangeByScoreAsync(messagesKey, lastReadScore, double.PositiveInfinity);

            foreach (var entry in newMessages)
            {
                var message = JsonConvert.DeserializeObject<Message>(entry);

                if (message.RecipientId == userId1 && !message.IsRead)
                {
                    message.IsRead = true;
                    
                    await _database.SortedSetRemoveAsync(messagesKey, entry);
                    var updatedMessageJson = JsonConvert.SerializeObject(message);
                    await _database.SortedSetAddAsync(messagesKey, updatedMessageJson, message.Timestamp.ToOADate());
                }
            }
            await SetLastReadTimestamp(userId1, userId2, DateTime.UtcNow);
        }catch (Exception ex)
        {
            Console.WriteLine($"Error in MarkNewMessagesAsRead: {ex.Message}");
            throw new Exception("An error occurred while reading messages.");
        }

    }

    private async Task<string> GetLastReadTimeStamp(long userId, string chatId)
    {
        try
        {
            var timeStampKey = $"{chatId}:{userId}:lastRead";
            var lastReadTimeStamp = await _database.StringGetAsync(timeStampKey);
        
            if(lastReadTimeStamp.IsNullOrEmpty)
            {
                return "No timestamp found"; 
            }
        
            return lastReadTimeStamp.ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetLastReadTimeStamp: {ex.Message}");
            throw new Exception("An error occurred while reading timestamp.");
        }
    }
    
    private async Task SetLastReadTimestamp(long userId, long recipientId, DateTime lastReadTimestamp)
    {
        try
        {
            var chatKey = GetChatKey(userId, recipientId);
            var lastReadKey = $"{chatKey}:{userId}:lastRead";

            var timestamp = lastReadTimestamp.ToOADate();
            await _database.StringSetAsync(lastReadKey, timestamp);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in SetLastReadTimestamp: {ex.Message}");
            throw new Exception("An error occurred while saving last read timestamp.");
        }
    }

    private string GetChatKey(long userId1, long userId2)
    {
        var minId = Math.Min(userId1, userId2);
        var maxId = Math.Max(userId1, userId2);
        return $"chat:{minId}:{maxId}";
    }
}