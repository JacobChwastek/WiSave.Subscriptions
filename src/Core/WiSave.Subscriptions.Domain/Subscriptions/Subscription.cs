using WiSave.Shared.EventStore.Aggregate;
using WiSave.Shared.Types;
using WiSave.Subscriptions.Contracts;
using WiSave.Subscriptions.Contracts.Commands;
using WiSave.Subscriptions.Contracts.Dtos;
using WiSave.Subscriptions.Contracts.Events;
using WiSave.Subscriptions.Domain.Subscriptions.Entities;
using WiSave.Subscriptions.Domain.Subscriptions.Specifications;
using WiSave.Subscriptions.Domain.Subscriptions.ValueObjects;

namespace WiSave.Subscriptions.Domain.Subscriptions;

internal sealed class Subscription : Aggregate<SubscriptionId>
{
    public Guid UserId { get; init; }
    public SubscriptionName? Name { get; private set; }
    public DateOnly StartDate { get; private set; }
    
    public DateOnly? EndDate { get; private set; }
    public SubscriptionStatus Status { get; private set; }
    public RenewalPolicy? RenewalPolicy { get; private set; }
    public TrialPeriod? Trial { get; private set; }

    private readonly List<SubscriptionPlan> _plans = [];
    public IReadOnlyList<SubscriptionPlan> Plans => _plans.AsReadOnly();

    public SubscriptionPlan? ActivePlan => _plans.FirstOrDefault(p => p.IsActive);

    public Subscription()
    {
    }

    private Subscription(Guid userId, string name, string planName, Money price, PeriodUnit periodUnit, int periodInterval, bool autoRenew, DateOnly startDate, bool isTrial, int? maxRenewals, int? trialDurationInDays)
    {
        new SubscriptionNameSpecification().Check(name);
        new SubscriptionPlanSpecification().Check(planName);
        new SubscriptionPeriodIntervalSpecification().Check(periodInterval);

        var subscriptionId = new SubscriptionId(Guid.CreateVersion7());
        var initialPlanId = Guid.CreateVersion7();

        var @event = new SubscriptionCreated(
            subscriptionId,
            userId,
            name,
            periodUnit,
            periodInterval,
            autoRenew,
            startDate,
            isTrial,
            maxRenewals,
            trialDurationInDays,
            new PlanDataDto(initialPlanId, planName, price)
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

        var initialPlan = new SubscriptionPlan(@event.Plan.Id, @event.Plan.Name, @event.Plan.Price, @event.StartDate, true);

        _plans.Add(initialPlan);

        Trial = @event is { IsTrial: true, TrialDurationInDays: not null, MaxRenewals: not null }
            ? new TrialPeriod(@event.StartDate, @event.TrialDurationInDays ?? 0)
            : null;
    }
}