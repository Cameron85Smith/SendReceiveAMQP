using Microsoft.Extensions.DependencyInjection;

// 17 Register the service.
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IMessageHandler, MessageHandler>();
        return services;
    }
}