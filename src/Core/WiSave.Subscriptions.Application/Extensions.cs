using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WiSave.Subscriptions.Domain;

namespace WiSave.Subscriptions.Application;

internal static class Extensions
{
    internal static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddDomain(configuration);
    }
}