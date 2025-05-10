using WiSave.Shared.EventStore.Aggregate;

namespace WiSave.Subscriptions.Domain.Subscriptions.Exceptions;

public class SubscriptionPeriodIntervalMustBePositiveException() : DomainException("Subscription period interval must be positive.");