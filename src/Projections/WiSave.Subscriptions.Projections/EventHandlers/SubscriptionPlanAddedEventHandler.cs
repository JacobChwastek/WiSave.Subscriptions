using MassTransit;
using WiSave.Subscriptions.Contracts.Events;
using WiSave.Subscriptions.Projections.Converters;
using WiSave.Subscriptions.Projections.Repository;

namespace WiSave.Subscriptions.Projections.EventHandlers;

internal class SubscriptionPlanAddedEventHandler(ISubscriptionRepository repository) : IConsumer<SubscriptionPlanAdded>
{
    public async Task Consume(ConsumeContext<SubscriptionPlanAdded> context)
    {
        var message = context.Message;

        var subscription = await repository.GetByIdAsync(message.SubscriptionId, context.CancellationToken);
        if (subscription == null)
        {
            throw new InvalidOperationException($"Subscription {message.SubscriptionId} not found");
        }

        subscription.ApplyPlanAdded(message);
        await repository.UpdateAsync(subscription, context.CancellationToken);
    }
}