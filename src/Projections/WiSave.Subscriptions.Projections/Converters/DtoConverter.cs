using WiSave.Subscriptions.Contracts.Dtos;
using WiSave.Subscriptions.Projections.Models;

namespace WiSave.Subscriptions.Projections.Converters;

public static class DtoConverter
{
    public static SubscriptionDto ToDto(this Subscription subscription)
    {
        return new SubscriptionDto(
            subscription.Id,
            subscription.Logo,
            subscription.Name,
            subscription.CurrentPlanName,
            subscription.CurrentPrice,
            subscription.PeriodUnit,
            subscription.PeriodInterval,
            subscription.AutoRenew,
            subscription.StartDate,
            subscription.EndDate,
            subscription.Status,
            subscription.IsTrial,
            subscription.MaxRenewals,
            subscription.TrialDurationInDays,
            subscription.Plans.Select(p => p.ToDto()).ToList(),
            subscription.ActivePlan?.ToDto(),
            subscription.TotalSpent,
            subscription.MonthlyValue,
            subscription.NextPaymentAmount,
            subscription.DaysActive,
            subscription.NextPaymentDate,
            subscription.IsInTrial,
            subscription.TrialDaysRemaining
        );

    }

    public static SubscriptionPlanDto ToDto(this SubscriptionPlan plan)
    {
        return new SubscriptionPlanDto(
            plan.Id,
            plan.Name,
            plan.IsActive,
            plan.EffectiveFrom,
            plan.CurrentPrice, 
            plan.PriceHistory.Select(ph => ph.ToDto()).ToList()
        );
    }

    public static PlanPriceHistoryDto ToDto(this PlanPriceHistory priceHistory)
    {
        return new PlanPriceHistoryDto(
            priceHistory.Price,
            priceHistory.EffectiveFrom
        );
    }
}