using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WiSave.Shared.EventStore.Marten;
using WiSave.Subscriptions.Domain.Subscriptions;

namespace WiSave.Subscriptions.Domain;

internal static class Extensions
{
    internal static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}