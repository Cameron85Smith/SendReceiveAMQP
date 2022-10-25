using Application.interfaces;
using Infrastructure.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;

// 9 Register the service.
namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMessenger(this IServiceCollection services)
        {
            // Lifetime is transient because the connection string in RabbitMQSender is disposed after every call.
            services.AddTransient<IMessageSender, RabbitMQSender>();
            return services;
        }

        public static IServiceCollection AddMessageReceiver(this IServiceCollection services)
        {
            // Register service.
            services.AddHostedService<RabbitMQReceiver>();
            return services;
        }
    }
}