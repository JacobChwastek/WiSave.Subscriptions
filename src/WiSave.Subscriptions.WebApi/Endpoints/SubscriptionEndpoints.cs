using MassTransit;
using MassTransit.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using WiSave.Subscriptions.Contracts.Commands;
using WiSave.Subscriptions.Contracts.Queries;
using WiSave.Subscriptions.MassTransit;
using WiSave.Subscriptions.WebApi.Services;

namespace WiSave.Subscriptions.WebApi.Endpoints;

public static class Subscriptions
{
    private static string Key => "api/subscriptions";

    public static RouteGroupBuilder MapSubscriptionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(Key).WithTags(Key);

        group.MapGet("", async ([AsParameters] GetSubscriptionsQuery query, [FromServices] IRequestClient<GetSubscriptionsQuery> client, [FromServices] IUserContextService userContext, CancellationToken cancellationToken) =>
            {
                var userId = userContext.GetUserId();

                if (string.IsNullOrWhiteSpace(userId))
                {
                    return Results.BadRequest("User ID is required");
                }
                
                query = query with
                {
                    UserId = userId
                };
                
                var response = await client.GetResponse<GetSubscriptionsQueryResult>(query, cancellationToken);

                return Results.Ok(response.Message);
            })
            .Produces<GetSubscriptionsQueryResult>();

        group.MapGet("{id:guid}", async ([FromRoute] Guid id, [FromServices] IRequestClient<GetSubscriptionQuery> client, CancellationToken cancellationToken) =>
            {
                var response = await client.GetResponse<GetSubscriptionQueryResult>(new GetSubscriptionQuery(id), cancellationToken);

                return Results.Ok(response);
            })
            .Produces<GetSubscriptionQueryResult>();

        group.MapPost("", async ([FromBody] CreateSubscription command, [FromServices] Bind<ISubscriptionBus, IPublishEndpoint> endpoint, CancellationToken token) =>
        {
            await endpoint.Value.Publish(command, token);
            return Results.Ok();
        });

        return group.WithTags("Subscriptions");
    }
}