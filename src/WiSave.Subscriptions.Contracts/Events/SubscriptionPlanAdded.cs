using WiSave.Shared.Types;

namespace WiSave.Subscriptions.Contracts.Events;

public record SubscriptionPlanAdded(
    SubscriptionId SubscriptionId,
    Guid PlanId,
    string PlanName,
    Money Price,
    DateOnly EffectiveFrom,
    bool MadeActive
);