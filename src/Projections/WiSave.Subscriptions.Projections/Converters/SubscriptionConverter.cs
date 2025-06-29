using WiSave.Subscriptions.Contracts.Events;
using WiSave.Subscriptions.Projections.Models;

namespace WiSave.Subscriptions.Projections.Converters;

internal static class SubscriptionConverter
{
    public static Subscription Project(this SubscriptionCreated @event)
    {
        var initialPlan = new SubscriptionPlan
        {
            Id = @event.Plan.Id,
            Name = @event.Plan.Name,
            IsActive = true,
            EffectiveFrom = @event.StartDate,
            PriceHistory =
            [
                new PlanPriceHistory
                {
                    Price = @event.Plan.Price,
                    EffectiveFrom = @event.StartDate
                }
            ]
        };

        return new Subscription
        {
            Id = @event.Id,
            Name = @event.Name,
            UserId = @event.UserId,
            PeriodUnit = @event.PeriodUnit,
            PeriodInterval = @event.PeriodInterval,
            AutoRenew = @event.AutoRenew,
            StartDate = @event.StartDate,
            Status = "Active",
            IsTrial = @event.IsTrial,
            MaxRenewals = @event.MaxRenewals,
            TrialDurationInDays = @event.TrialDurationInDays,
            Plans = [initialPlan]
        };
    }

    public static void ApplyPlanAdded(this Subscription subscription, SubscriptionPlanAdded @event)
    {
        if (@event.MadeActive)
        {
            subscription.Plans.ForEach(p => p.IsActive = false);
        }

        var newPlan = new SubscriptionPlan
        {
            Id = @event.PlanId,
            Name = @event.PlanName,
            IsActive = @event.MadeActive,
            EffectiveFrom = @event.EffectiveFrom,
            PriceHistory =
            [
                new PlanPriceHistory
                {
                    Price = @event.Price,
                    EffectiveFrom = @event.EffectiveFrom
                }
            ]
        };

        subscription.Plans.Add(newPlan);
    }

    public static void ApplyPlanSwitched(this Subscription subscription, SubscriptionPlanSwitched @event)
    {
        subscription.Plans.ForEach(p => p.IsActive = false);
        
        var targetPlan = subscription.Plans.FirstOrDefault(p => p.Id == @event.NewActivePlanId);
        if (targetPlan != null)
        {
            targetPlan.IsActive = true;
        }
    }

    public static void ApplyPriceHistoryModified(this Subscription subscription, SubscriptionPlanPriceHistoryModified @event)
    {
        var plan = subscription.Plans.FirstOrDefault(p => p.Id == @event.PlanId);
        if (plan == null) 
            return;
        
        plan.PriceHistory.RemoveAll(ph => ph.EffectiveFrom == @event.EffectiveFrom);
        
        plan.PriceHistory.Add(new PlanPriceHistory
        {
            Price = @event.NewPrice,
            EffectiveFrom = @event.EffectiveFrom
        });
    }
}