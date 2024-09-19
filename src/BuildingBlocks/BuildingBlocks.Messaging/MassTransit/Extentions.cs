using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Messaging.MassTransit;

public record MessageBroker(string Host, string UserName, string Password);

public static class Extentions
{
    public static IServiceCollection AddMessageBroker
        (this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        var messageBroker = configuration.GetSection(nameof(MessageBroker))
            .Get<MessageBroker>() ?? throw new Exception($"{nameof(MessageBroker)} isn't found");
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();
            if (assembly != null) config.AddConsumers(assembly);
            config.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(messageBroker.Host), host =>
                {
                    host.Username(messageBroker.UserName);
                    host.Password(messageBroker.Password);
                });
                configurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
