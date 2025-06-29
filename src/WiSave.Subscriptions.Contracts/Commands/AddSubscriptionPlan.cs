using WiSave.Shared.Types;

namespace WiSave.Subscriptions.Contracts.Commands;

public record AddSubscriptionPlan(
    SubscriptionId SubscriptionId,
    string PlanName,
    Money Price,
    DateTime EffectiveFrom,
    bool IsActive = false
);
