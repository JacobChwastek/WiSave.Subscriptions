using WiSave.Shared.Types;

namespace WiSave.Subscriptions.Contracts.Events;

public sealed record SubscriptionCreated(
    SubscriptionId Id,
    string Name,
    string Plan,
    Money Money,
    PeriodUnit PeriodUnit,
    int PeriodInterval,
    bool AutoRenew,
    DateOnly StartDate,
    bool IsTrial,
    int? MaxRenewals,
    int? TrialDurationInDays
);