using WiSave.Subscriptions.Contracts.Events;
using WiSave.Subscriptions.Projections.Models;

namespace WiSave.Subscriptions.Projections.Converters;

internal static class SubscriptionConverter
{
    public static Subscription Project(this SubscriptionCreated @event)
    {
        return new Subscription
        {
            Id = @event.Id,
            Name = @event.Name,
            UserId = @event.UserId,
            Plan = @event.Plan,
            Money = @event.Money,
            PeriodUnit = @event.PeriodUnit,
            PeriodInterval = @event.PeriodInterval,
            AutoRenew = @event.AutoRenew,
            StartDate = @event.StartDate,
            IsTrial = @event.IsTrial,
            MaxRenewals = @event.MaxRenewals,
            TrialDurationInDays = @event.TrialDurationInDays
        };
    }
}