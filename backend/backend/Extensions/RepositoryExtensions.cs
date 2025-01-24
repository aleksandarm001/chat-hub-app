using backend.Interfaces.Repositories;
using backend.Interfaces.Services;
using backend.Repositories;
using backend.Services;

namespace backend.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IMessageRepository, RedisMessageRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        return services;

    }
}