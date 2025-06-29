using MassTransit;
using WiSave.Subscriptions.Contracts.Events;
using WiSave.Subscriptions.Projections.Converters;
using WiSave.Subscriptions.Projections.Repository;

namespace WiSave.Subscriptions.Projections.EventHandlers;

internal class SubscriptionPlanSwitchedEventHandler(ISubscriptionRepository repository) : IConsumer<SubscriptionPlanSwitched>
{
    public async Task Consume(ConsumeContext<SubscriptionPlanSwitched> context)
    {
        var message = context.Message;

        var subscription = await repository.GetByIdAsync(message.SubscriptionId, context.CancellationToken);
        if (subscription == null)
        {
            throw new InvalidOperationException($"Subscription {message.SubscriptionId} not found");
        }

        subscription.ApplyPlanSwitched(message);
        await repository.UpdateAsync(subscription, context.CancellationToken);
    }
}