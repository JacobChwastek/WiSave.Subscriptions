using WiSave.Shared.Types;

namespace WiSave.Subscriptions.Contracts.Dtos;

public record SubscriptionDto(
    Guid Id,
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