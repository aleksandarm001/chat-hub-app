using backend.Models;
using backend.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace backend.Interfaces.Services;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(int userId);
    Task<User?> GetUserByEmailAsync(string email);
    Task<bool> AddUserAsync(RegistrationModel credentials);
    Task<bool> UserExistsAsync(string email);
    Task<IEnumerable<User>> GetUsers();
}