using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Extensions;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}
    
    public DbSet<User> Users { get; set; }
    
}