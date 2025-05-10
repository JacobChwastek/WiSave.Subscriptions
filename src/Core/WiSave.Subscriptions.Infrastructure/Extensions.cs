using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WiSave.Shared.EventStore.Marten;
using WiSave.Shared.EventStore.Marten.Repository;
using WiSave.Shared.MassTransit;
using WiSave.Shared.OpenTelemetry.OpenTelemetry;
using WiSave.Subscriptions.Application;
using WiSave.Subscriptions.Application.CommandHandlers;
using WiSave.Subscriptions.Application.Sagas;
using WiSave.Subscriptions.Domain.Subscriptions;
using WiSave.Subscriptions.Infrastructure.OpenTelemetry;
using WiSave.Subscriptions.MassTransit;

namespace WiSave.Subscriptions.Infrastructure;

internal static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var telemetryOptions = configuration
            .GetSection("OpenTelemetry")
            .Get<TelemetryOptions>() ?? new TelemetryOptions();

        var rabbitMqConfiguration = configuration
            .GetSection("Messaging:Subscriptions")
            .Get<RabbitMqConfiguration>() ?? new RabbitMqConfiguration();

        services
            .AddSingleton(telemetryOptions)
            .AddNpgsqlDataSource(configuration.GetConnectionString("EventStore") ?? string.Empty)
            .AddSingleton<IActivityScope, ActivityScope>();

        services
            .AddMarten<ISubscriptionBus>(configuration, options => { options.DisableNpgsqlLogging = true; })
            .UseNpgsqlDataSource();

        services
            .AddOpenTelemetryInfrastructure(configuration, environment)
            .AddMartenRepository<Subscription>()
            .AddMessaging<ISubscriptionBus>(rabbitMqConfiguration, consumerTypes: [typeof(CreateSubscriptionHandler), typeof(DelayedCommandSaga)])
            .AddApplication(configuration);

        return services;
    }
}