using backend.Hubs;
using backend.Interfaces.Services;
using backend.Models;
using backend.Services;

namespace backend.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<ITokenGenerator, TokenGenerator>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUserService, UserService>();
        //SignalR Hubs or other services (if needed, add here)
        return services;

    }
}