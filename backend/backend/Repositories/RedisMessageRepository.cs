using System.Text.Json;
using backend.Interfaces.Repositories;

using StackExchange.Redis;

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
    
    public async Task SaveMessageAsync(string user, string message)
    {
        var messageData = new { User = user, Message = message, Timestamp = DateTime.UtcNow };
        var jsonData = JsonSerializer.Serialize(messageData);

        await _database.ListRightPushAsync("chat_messages", jsonData);
    }

    public async Task<List<string>> GetMessages()
    {
        var messages = await _database.ListRangeAsync("chat_messages");
        return messages.Select(m => m.ToString()).ToList();
    }
}