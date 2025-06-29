using WiSave.Shared.Types;

namespace WiSave.Subscriptions.Contracts.Events;

public record SubscriptionPlanPriceHistoryModified(
    SubscriptionId SubscriptionId,
    Guid PlanId,
    Money NewPrice,
    DateOnly EffectiveFrom
);