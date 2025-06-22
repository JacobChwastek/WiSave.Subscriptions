using WiSave.Shared.EventStore.Aggregate;
using WiSave.Shared.Types;
using WiSave.Subscriptions.Contracts;
using WiSave.Subscriptions.Contracts.Commands;
using WiSave.Subscriptions.Contracts.Events;
using WiSave.Subscriptions.Domain.Subscriptions.Specifications;
using WiSave.Subscriptions.Domain.Subscriptions.ValueObjects;

namespace WiSave.Subscriptions.Domain.Subscriptions;

internal sealed class Subscription : Aggregate<SubscriptionId>
{
    public Guid UserId { get; init; }
    public SubscriptionName Name { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly? EndDate { get; private set; }
    public SubscriptionStatus Status { get; private set; }
    public RenewalPolicy RenewalPolicy { get; private set; }
    public TrialPeriod? Trial { get; private set; }

    public Subscription()
    {
    }

    private Subscription(Guid userId, string name, string plan, Money price, PeriodUnit periodUnit, int periodInterval, bool autoRenew, DateOnly startDate, bool isTrial, int? maxRenewals,
        int? trialDurationInDays)
    {
        new SubscriptionNameSpecification().Check(name);
        new SubscriptionPlanSpecification().Check(plan);
        new SubscriptionPeriodIntervalSpecification().Check(periodInterval);

        var @event = new SubscriptionCreated(
            new SubscriptionId(Guid.CreateVersion7()),
            userId,
            name,
            plan,
            price,
            periodUnit,
            periodInterval,
            autoRenew,
            startDate,
            isTrial,
            maxRenewals,
            trialDurationInDays
        );

        Apply(@event);
        Enqueue(@event);
    }

    public static Subscription Create(CreateSubscription command) =>
        new(command.UserId, command.Name, command.Plan, command.Money, command.PeriodUnit, command.PeriodInterval, command.AutoRenew, DateOnly.FromDateTime(command.StartDate), command.IsTrial,
            command.MaxRenewals, command.TrialDurationInDays);

    private void Apply(SubscriptionCreated @event)
    {
        Id = @event.Id;
        Name = new SubscriptionName(@event.Name);
        StartDate = @event.StartDate;
        Status = SubscriptionStatus.Active;
        RenewalPolicy = new RenewalPolicy(@event.AutoRenew, @event.MaxRenewals ?? 0);

        if (@event.IsTrial && @event.TrialDurationInDays.HasValue && @event.MaxRenewals.HasValue)
        {
            Trial = new TrialPeriod(@event.StartDate, @event.TrialDurationInDays ?? 0);
        }
        else
        {
            Trial = null;
        }
    }
}