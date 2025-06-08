using MassTransit;
using WiSave.Subscriptions.Contracts.Events;
using WiSave.Subscriptions.Projections.Converters;
using WiSave.Subscriptions.Projections.Repository;

namespace WiSave.Subscriptions.Projections.EventHandlers;

internal class SubscriptionCreatedEventHandler(ISubscriptionRepository repository): IConsumer<SubscriptionCreated>
{
    public async Task Consume(ConsumeContext<SubscriptionCreated> context)
    {
        var message = context.Message;

        var subscription = await repository.GetByIdAsync(message.Id, context.CancellationToken);

        if (subscription is not null)
        {
            throw new Exception("Subscription already exists exception");
        }

        await repository.InsertAsync(message.Project(), context.CancellationToken);
    }
}