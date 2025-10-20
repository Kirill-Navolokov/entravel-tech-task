using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderProcessingService.Messaging.Config;
using OrderProcessingService.Messaging.Services;

namespace OrderProcessingService.Messaging;

public static class Extensions
{
    public static IServiceCollection AddMessaging(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMQ"))
            .Configure<MessageQueueOptions>(configuration.GetSection("MessageQueues"));

        services.AddSingleton<IMessagingService, RabbitMessagingService>();
        
        return services;
    }
}