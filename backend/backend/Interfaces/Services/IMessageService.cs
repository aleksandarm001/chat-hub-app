namespace backend.Interfaces.Services;

public interface IMessageService
{
    Task AddMessageAsync(string user, string message);
    Task<List<string>> GetMessages();
}