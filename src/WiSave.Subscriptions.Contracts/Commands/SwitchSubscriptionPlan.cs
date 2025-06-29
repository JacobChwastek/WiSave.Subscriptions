namespace WiSave.Subscriptions.Contracts.Commands;

public record SwitchSubscriptionPlan(
    SubscriptionId SubscriptionId,
    Guid PlanId
);