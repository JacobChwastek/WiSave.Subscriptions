using MassTransit;
using WiSave.Subscriptions.Contracts.Dtos;
using WiSave.Subscriptions.Contracts.Queries;
using WiSave.Subscriptions.Projections.Converters;
using WiSave.Subscriptions.Projections.Repository;

namespace WiSave.Subscriptions.Projections.QueryHandlers;

internal class GetSubscriptionQueryHandler(ISubscriptionRepository repository) : IConsumer<GetSubscriptionQuery>
{
    public async Task Consume(ConsumeContext<GetSubscriptionQuery> context)
    {
        var message = context.Message;
        var subscription = await repository.GetByIdAsync(message.Id, context.CancellationToken);

        if (subscription == null)
        {
            await context.RespondAsync(new GetSubscriptionQueryResult(null));
            return;
        }

        var dto = subscription.ToDto();

        await context.RespondAsync(new GetSubscriptionQueryResult(dto));
    }
}