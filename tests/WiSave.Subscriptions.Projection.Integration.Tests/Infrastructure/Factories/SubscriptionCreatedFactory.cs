using WiSave.Shared.Types;
using WiSave.Subscriptions.Contracts.Events;

namespace WiSave.Subscriptions.Projection.Integration.Tests.Infrastructure.Factories;

public static class SubscriptionCreatedFactory
{
    public static SubscriptionCreated CreateValid()
    {
        return new SubscriptionCreated(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Jakub",
            "Pro Plan",
            new Money(29.99m, Currency.EUR),
            PeriodUnit.Month,
            1,
            true,
            DateOnly.FromDateTime(DateTime.UtcNow),
            false,
            null,
            null
        );
    }
}