using MassTransit;
using WiSave.Shared.EventStore.Marten.Repository;
using WiSave.Subscriptions.Contracts.Commands;
using WiSave.Subscriptions.Domain.Subscriptions;

namespace WiSave.Subscriptions.Application.CommandHandlers;

internal class CreateSubscriptionHandler(IMartenRepository<Subscription> repository) : IConsumer<CreateSubscription>
{
    public async Task Consume(ConsumeContext<CreateSubscription> context)
    {
        var message = context.Message;

        var subscription = Subscription.Create(message);

        await repository.Add(subscription.Id, subscription, context.CancellationToken);
    }
}