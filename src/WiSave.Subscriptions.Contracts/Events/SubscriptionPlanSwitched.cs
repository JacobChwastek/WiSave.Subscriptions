namespace WiSave.Subscriptions.Contracts.Events;

public record SubscriptionPlanSwitched(
    SubscriptionId SubscriptionId,
    Guid NewActivePlanId,
    DateOnly SwitchedOn
);
