using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WiSave.Subscriptions.Projections;

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
    .ConfigureLogging((logging) =>
    {
        logging.AddConsole();
    })
    .ConfigureServices((context, services) =>
    {
        services.AddProjections(context.Configuration);
    });
   

var host = builder.Build();

using (host)
{
    await host.RunAsync();
}