using backend.Models;

namespace backend.Interfaces.Repositories;

public interface IMessageRepository
{
    Task CreateChat(long userId1, long userId2);
    Task CreateMessage(long userId1, long userId2, string content);
    Task<List<Message>> GetMessages(long userId1, long userId2);
    Task ReadMessage(long userId1, long userId2);
    
}