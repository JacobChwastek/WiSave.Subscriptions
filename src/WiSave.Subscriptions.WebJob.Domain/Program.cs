using MassTransit.Courier.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WiSave.Shared.EventStore.Marten.Repository;
using WiSave.Subscriptions.Infrastructure;


var builder = new HostBuilder();

builder
    .UseEnvironment(Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development");

builder
    .ConfigureAppConfiguration(config =>
        {
            config.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
        }
    )
    .ConfigureServices((context, services) =>
    {
        services.AddInfrastructure(context.Configuration, context.HostingEnvironment);
    });
   

var host = builder.Build();

using (host)
{
    await host.RunAsync();
}