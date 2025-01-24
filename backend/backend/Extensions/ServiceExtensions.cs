using backend.Interfaces.Services;
using backend.Services;

namespace backend.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IMessageService, MessageService>();
        
        //SignalR Hubs or other services (if needed, add here)
        return services;

    }
}