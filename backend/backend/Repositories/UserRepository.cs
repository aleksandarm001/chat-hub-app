using backend.Extensions;
using backend.Interfaces.Repositories;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<User> _users;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
        _users = context.Set<User>();
    }
    
    public async Task<User?> GetUser(int userId)
    {
        try
        {
            return await _users.FindAsync(userId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetUser: {ex.Message}");
            throw;
        }
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        try
        {
            return await _users.FirstOrDefaultAsync(user => user.Email == email);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetUserByEmail: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> AddUser(User user)
    {
        try
        {
            await _users.AddAsync(user);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AddUser: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> UserExists(string email)
    {
        try
        {
            return await _users.AnyAsync(user => user.Email == email);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UserExists: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        try
        {
            return await _users.ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetUsers: {ex.Message}");
            throw;
        }
    }
}