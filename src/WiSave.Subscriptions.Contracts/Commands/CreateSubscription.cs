using WiSave.Shared.Types;

namespace WiSave.Subscriptions.Contracts.Commands;

public record CreateSubscription(
    Guid UserId,
    string Name,
    string Plan,
    Money Money,
    PeriodUnit PeriodUnit,
    int PeriodInterval,
    bool AutoRenew,
    DateTime StartDate,
    bool IsTrial,
    int? MaxRenewals,
    int? TrialDurationInDays
);