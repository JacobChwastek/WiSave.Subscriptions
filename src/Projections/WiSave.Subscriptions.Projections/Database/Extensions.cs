using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using WiSave.Subscriptions.Projections.Configuration;

namespace WiSave.Subscriptions.Projections.Database;

internal static class Extensions
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection("Mongo").Get<MongoDbOptions>()!;
        services.AddSingleton(options);

        services.AddSingleton<IMongoClient>(_ =>
            new MongoClient(options.ConnectionString));

        services.AddScoped(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            var opts = sp.GetRequiredService<MongoDbOptions>();
            return client.GetDatabase(opts.DatabaseName);
        });

        return services;
    }
}