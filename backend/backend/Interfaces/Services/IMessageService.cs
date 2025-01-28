using backend.Models;

namespace backend.Interfaces.Services;

public interface IMessageService
{
    Task CreateChat(long userId1, long userId2);
    Task SendMessage(long userId1, long userId2, string content);
    Task<List<Message>> GetMessages(long userId1, long userId2);
    Task ReadMessages(long userId1, long userId2);
}