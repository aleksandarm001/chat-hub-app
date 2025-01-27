using backend.Interfaces.Repositories;
using backend.Interfaces.Services;
using backend.Models;
using backend.Models.Auth;

namespace backend.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        try
        {
            return await _userRepository.GetUser(userId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetUserByIdAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        try
        {
            return await _userRepository.GetUserByEmail(email);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetUserByEmailAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> AddUserAsync(RegistrationModel credentials)
    {
        try
        {
            string hashedpassword = PasswordHasher.HashPassword(credentials.Password);
            User user = new User(credentials.Username, credentials.Email, hashedpassword,
                credentials.Firstname, credentials.Lastname);
            return await _userRepository.AddUser(user);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AddUserAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> UserExistsAsync(string email)
    {
        try
        {
            return await _userRepository.UserExists(email);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UserExistsAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        try
        {
            return await _userRepository.GetUsers();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetUsersAsync: {ex.Message}");
            throw;
        }
    }
}
    