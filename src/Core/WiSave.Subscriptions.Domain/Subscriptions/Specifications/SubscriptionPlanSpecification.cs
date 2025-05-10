using WiSave.Shared.EventStore.Aggregate;
using WiSave.Subscriptions.Domain.Subscriptions.Exceptions;

namespace WiSave.Subscriptions.Domain.Subscriptions.Specifications;


public class SubscriptionPlanSpecification : ISpecification<string>
{
    public bool IsSatisfiedBy(string candidate) => !string.IsNullOrWhiteSpace(candidate);

    public void Check(string candidate)
    {
        if (!IsSatisfiedBy(candidate))
            throw new SubscriptionPlanNameMustNotBeEmptyException();
    }
}