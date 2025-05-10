using WiSave.Shared.EventStore.Aggregate;
using WiSave.Subscriptions.Domain.Subscriptions.Exceptions;

namespace WiSave.Subscriptions.Domain.Subscriptions.Specifications;


public class SubscriptionPeriodIntervalSpecification : ISpecification<int>
{
    public bool IsSatisfiedBy(int candidate) => candidate > 0;

    public void Check(int candidate)
    {
        if (!IsSatisfiedBy(candidate))
            throw new SubscriptionPeriodIntervalMustBePositiveException();
    }
}