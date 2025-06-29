using WiSave.Shared.Types;

namespace WiSave.Subscriptions.Contracts.Dtos;

public record SubscriptionDto(
    Guid Id,
    string Logo,
    string Name,
    string? CurrentPlanName,
    Money? CurrentPrice,
    PeriodUnit PeriodUnit,
    int PeriodInterval,
    bool AutoRenew,
    DateOnly StartDate,
    DateOnly? EndDate,
    string Status,
    bool IsTrial,
    int? MaxRenewals,
    int? TrialDurationInDays,
    List<SubscriptionPlanDto> Plans,
    SubscriptionPlanDto? ActivePlan,
    Money? TotalSpent,
    Money? MonthlyValue,
    Money? NextPaymentAmount,
    int DaysActive,
    DateOnly? NextPaymentDate,
    bool IsInTrial,
    int? TrialDaysRemaining
);