using MassTransit;
using WiSave.Subscriptions.Contracts.Events;

namespace WiSave.Subscriptions.Application.Sagas;

public class DelayedCommandSaga : IConsumer<SubscriptionCreated>
{
    public async Task Consume(ConsumeContext<SubscriptionCreated> context)
    {
        var message = context.Message;
    }
}