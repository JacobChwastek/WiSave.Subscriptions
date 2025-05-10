using WiSave.Shared.EventStore.Aggregate;

namespace WiSave.Subscriptions.Domain.Subscriptions.Exceptions;

public class SubscriptionNameMustNotBeEmptyException() : DomainException("Subscription name cannot be empty.");