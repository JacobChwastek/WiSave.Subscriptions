using WiSave.Shared.EventStore.Aggregate;

namespace WiSave.Subscriptions.Domain.Subscriptions.Exceptions;

public class SubscriptionPlanNameMustNotBeEmptyException() : DomainException("Subscription plan name cannot be empty.");