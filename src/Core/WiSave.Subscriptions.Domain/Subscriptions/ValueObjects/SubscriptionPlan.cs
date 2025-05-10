using WiSave.Shared.Types;
using WiSave.Subscriptions.Domain.Subscriptions.Exceptions;

namespace WiSave.Subscriptions.Domain.Subscriptions.ValueObjects;

public record SubscriptionPlan
{
    public SubscriptionPlan()
    {
    }

    public SubscriptionPlan(string name, Money price)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new SubscriptionPlanNameMustNotBeEmptyException();
        }

        Name = name;
        Price = price;
    }

    public string Name { get; init; }
    public Money Price { get; set; }
}