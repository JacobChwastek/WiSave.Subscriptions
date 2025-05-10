using WiSave.Shared.EventStore.Aggregate;

namespace WiSave.Subscriptions.Contracts;

public record SubscriptionId(Guid Id) : AggregateRootId(Id)
{
    public static implicit operator Guid(SubscriptionId id) => id.Id;
    public static implicit operator SubscriptionId(Guid id) => new(id);
}