using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WiSave.Shared.MassTransit;
using WiSave.Shared.OpenTelemetry.OpenTelemetry;
using WiSave.Subscriptions.MassTransit;
using WiSave.Subscriptions.Projections.Database;
using WiSave.Subscriptions.Projections.EventHandlers;
using WiSave.Subscriptions.Projections.QueryHandlers;
using WiSave.Subscriptions.Projections.Repository;

namespace WiSave.Subscriptions.Projections;

internal static class Extensions
{
    public static IServiceCollection AddProjections(this IServiceCollection services, IConfiguration configuration)
    {
        var telemetryOptions = configuration
            .GetSection("OpenTelemetry")
            .Get<TelemetryOptions>() ?? new TelemetryOptions();

        
        var rabbitMqConfiguration = configuration
            .GetSection("Messaging:Subscriptions")
            .Get<RabbitMqConfiguration>() ?? new RabbitMqConfiguration();


        var eventHandlers = new[]
        {
            typeof(SubscriptionCreatedEventHandler),
            typeof(GetSubscriptionQueryHandler),
            typeof(GetSubscriptionsQueryHandler),
        };
        
        services
                
            .AddSingleton(telemetryOptions)
            .AddMessaging<ISubscriptionBus>(rabbitMqConfiguration, consumerTypes: eventHandlers)
            .AddMongoDb(configuration)
            .AddScoped<ISubscriptionRepository, SubscriptionRepository>();

        return services;
    }
}