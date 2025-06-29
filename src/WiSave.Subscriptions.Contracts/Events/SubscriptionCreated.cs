using WiSave.Shared.Types;
using WiSave.Subscriptions.Contracts.Dtos;

namespace WiSave.Subscriptions.Contracts.Events;

public sealed record SubscriptionCreated(
    SubscriptionId Id,
    Guid UserId,
    string Name,
    PeriodUnit PeriodUnit,
    int PeriodInterval,
    bool AutoRenew,
    DateOnly StartDate,
    bool IsTrial,
    int? MaxRenewals,
    int? TrialDurationInDays,
    PlanDataDto Plan
);