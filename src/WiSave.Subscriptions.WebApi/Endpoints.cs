using MassTransit;
using MassTransit.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using WiSave.Shared.Types;
using WiSave.Subscriptions.Contracts;
using WiSave.Subscriptions.Contracts.Commands;
using WiSave.Subscriptions.MassTransit;

namespace WiSave.Subscriptions.WebApi;

public static class Endpoints
{
    private static string Key => "subscriptions";
    public static RouteGroupBuilder UseEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(Key).WithTags(Key);

        group.MapGet("", async () => { return Results.Ok(); });

        group.MapPost("", async ([FromBody] CreateSubscription command, [FromServices] Bind<ISubscriptionBus, IPublishEndpoint> publishEndpoint, CancellationToken token) =>
        {
            await publishEndpoint.Value.Publish(command, token);
            return Results.Ok();
        });

        return group;
    }
}