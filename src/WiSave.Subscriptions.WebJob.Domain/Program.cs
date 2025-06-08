using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using WiSave.Subscriptions.Infrastructure;

var builder = new HostBuilder();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

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