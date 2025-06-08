using WiSave.Subscriptions.Contracts.Dtos;

namespace WiSave.Subscriptions.Contracts.Queries;

public record GetSubscriptionsQuery(
    string? Name = null,
    string? Plan = null,
    bool? IsTrial = null
) : PagedQuery;

public record GetSubscriptionsQueryResult(PagedResult<SubscriptionDto> Subscriptions);