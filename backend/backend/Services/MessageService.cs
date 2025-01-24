using backend.Interfaces.Repositories;
using backend.Interfaces.Services;
using StackExchange.Redis;

namespace backend.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;

    public MessageService(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }
    
    public async Task AddMessageAsync(string user, string message)
    {
        await _messageRepository.SaveMessageAsync(user, message);
    }

    public async Task<List<string>> GetMessages()
    {
        return await _messageRepository.GetMessages();
    }
}