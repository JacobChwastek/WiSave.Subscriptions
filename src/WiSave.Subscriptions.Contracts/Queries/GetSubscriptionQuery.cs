using WiSave.Subscriptions.Contracts.Dtos;

namespace WiSave.Subscriptions.Contracts.Queries;

public record GetSubscriptionQuery(Guid Id);

public record GetSubscriptionQueryResult(SubscriptionDto? Subscription);