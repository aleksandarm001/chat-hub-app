using backend.Models;

namespace backend.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetUser(int userId);
    Task<User?> GetUserByEmail(string email);
    Task<bool> AddUser(User user);
    Task<bool> UserExists(string email);
    Task<IEnumerable<User>> GetUsers();
}