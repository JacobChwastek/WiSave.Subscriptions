using MassTransit.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.OpenTelemetry;
using WiSave.Shared.OpenTelemetry.OpenTelemetry;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using ResourceBuilder = WiSave.Shared.OpenTelemetry.OpenTelemetry.ResourceBuilder;

namespace WiSave.Subscriptions.Infrastructure.OpenTelemetry;

public static class Extensions
{
    public static IServiceCollection AddOpenTelemetryInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var telemetryOptions = configuration
            .GetSection("OpenTelemetry")
            .Get<TelemetryOptions>() ?? new TelemetryOptions();

        services.AddSingleton(telemetryOptions);

        services.AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                var resourceBuilder = ResourceBuilder.CreateResourceBuilder(environment)
                    .AddEnvironmentVariableDetector();

                tracing
                    .SetResourceBuilder(resourceBuilder)
                    .AddHttpClientInstrumentation()
                    .AddSource(environment.ApplicationName)
                    .AddSource(DiagnosticHeaders.DefaultListenerName) 
                    .AddOtlpExporter(opt =>
                    {
                        opt.Endpoint = new Uri(telemetryOptions.TraceEndpoint);
                        opt.Protocol = OtlpExportProtocol.HttpProtobuf;
                        opt.Headers = telemetryOptions.Headers;
                    });
            });

        var logger = CreateLogger(telemetryOptions, environment);
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(logger);
        });
        

        return services;
    }

    private static Logger CreateLogger(this TelemetryOptions telemetryOptions, IHostEnvironment environment)
    {
        var headersDictionary = new Dictionary<string, string>();

        if (string.IsNullOrEmpty(telemetryOptions.Headers))
            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.OpenTelemetry(options =>
                {
                    options.Endpoint = telemetryOptions.LoggingEndpoint;
                    options.Protocol = OtlpProtocol.HttpProtobuf;
                    options.ResourceAttributes = new Dictionary<string, object>
                    {
                        ["service.name"] = "WiSave.Subscriptions",
                        ["deployment.environment"] = environment.EnvironmentName
                    };
                    options.Headers = headersDictionary;
                })
                .CreateLogger();
        
        var headerPairs = telemetryOptions.Headers.Split(';', StringSplitOptions.RemoveEmptyEntries);

        foreach (var headerPair in headerPairs)
        {
            var separatorIndex = headerPair.IndexOf('=');
            if (separatorIndex > 0 && separatorIndex < headerPair.Length - 1)
            {
                var key = headerPair[..separatorIndex].Trim();
                var value = headerPair[(separatorIndex + 1)..].Trim();
                headersDictionary[key] = value;
            }
            else
            {
                throw new ArgumentException($"Invalid header format: '{headerPair}'. Expected format: 'Key=Value'.");
            }
        }

        return new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.OpenTelemetry(options =>
            {
                options.Endpoint = telemetryOptions.LoggingEndpoint;
                options.Protocol = OtlpProtocol.HttpProtobuf;
                options.ResourceAttributes = new Dictionary<string, object>
                {
                    ["service.name"] = "WiSave.Subscriptions",
                    ["deployment.environment"] = environment.EnvironmentName
                };
                options.Headers = headersDictionary;
            })
            .CreateLogger();
    }
}