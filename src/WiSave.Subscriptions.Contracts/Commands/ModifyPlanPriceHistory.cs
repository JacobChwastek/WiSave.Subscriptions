using WiSave.Shared.Types;

namespace WiSave.Subscriptions.Contracts.Commands;

public record ModifyPlanPriceHistory(
    SubscriptionId SubscriptionId,
    Guid PlanId,
    Money NewPrice,
    DateTime EffectiveFrom
);