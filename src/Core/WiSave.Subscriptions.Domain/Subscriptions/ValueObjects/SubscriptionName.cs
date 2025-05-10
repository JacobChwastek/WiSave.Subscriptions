using WiSave.Subscriptions.Domain.Subscriptions.Exceptions;

namespace WiSave.Subscriptions.Domain.Subscriptions.ValueObjects;

public record SubscriptionName
{
    public SubscriptionName()
    {
    }

    public SubscriptionName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new SubscriptionNameMustNotBeEmptyException();
        }

        Name = name;
    }

    public string Name { get; init; }
};