using backend.Interfaces.Repositories;
using backend.Interfaces.Services;
using backend.Models;
using StackExchange.Redis;

namespace backend.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;

    public MessageService(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }
    

    public async Task CreateChat(long userId1, long userId2)
    {
        try
        {
            await _messageRepository.CreateChat(userId1, userId2);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in CreateChat: {ex.Message}");
            throw new Exception("An error occurred while creating the chat.");
        }
    }

    public async Task SendMessage(long userId1, long userId2, string content)
    {
        try
        {
            await _messageRepository.CreateMessage(userId1, userId2, content);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in SendMessage: {ex.Message}");
            throw new Exception("An error occurred while creating message.");
        }
    }

    public Task<List<Message>> GetMessages(long userId1, long userId2)
    {
        try
        {
            return _messageRepository.GetMessages(userId1, userId2);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetMessages: {ex.Message}");
            throw new Exception("An error occurred while GetMessages.");
        }
    }

    public Task ReadMessages(long userId1, long userId2)
    {
        try
        {
            return _messageRepository.ReadMessage(userId1, userId2);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in ReadMessages: {ex.Message}");
            throw new Exception("An error occurred while ReadMessages.");
        }
    }
}