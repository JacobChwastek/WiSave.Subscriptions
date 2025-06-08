using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace WiSave.Subscriptions.Projection.Integration.Tests.Infrastructure.MassTransit;

public static class Extensions
{
    public static IServiceCollection AddMassTransitTestHarnessWithConsumers(this IServiceCollection services, string connectionString, params Type[] consumerTypes)
    {
        var rabbitMqUri = new Uri(connectionString);
        return services.AddMassTransitTestHarness(x =>
        {
            foreach (var consumerType in consumerTypes)
            {
                x.AddConsumer(consumerType);
            }

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqUri);
                cfg.ConfigureEndpoints(context);
            });
        });
    }
}