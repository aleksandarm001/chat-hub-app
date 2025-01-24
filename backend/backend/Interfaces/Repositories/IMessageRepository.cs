namespace backend.Interfaces.Repositories;

public interface IMessageRepository
{
    Task SaveMessageAsync(string user, string message);
    Task<List<string>> GetMessages();
}