using WiSave.Shared.Types;
using WiSave.Subscriptions.Domain.Subscriptions.ValueObjects;

namespace WiSave.Subscriptions.Domain.Subscriptions.Entities;

public class SubscriptionPlan
{
    public Guid Id { get; init; }
    public string Name { get; private set; }
    public bool IsActive { get; private set; }
    public DateOnly EffectiveFrom { get; private set; }
    private readonly List<PlanPriceHistory> _priceHistory = [];
    public IReadOnlyList<PlanPriceHistory> PriceHistory => _priceHistory.AsReadOnly();

    public Money CurrentPrice => _priceHistory
        .Where(p => p.EffectiveFrom <= DateOnly.FromDateTime(DateTime.UtcNow))
        .OrderByDescending(p => p.EffectiveFrom)
        .First().Price;

    public SubscriptionPlan()
    {
    }

    internal SubscriptionPlan(Guid id, string name, Money initialPrice, DateOnly effectiveFrom, bool isActive = false)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        EffectiveFrom = effectiveFrom;
        _priceHistory.Add(new PlanPriceHistory(initialPrice, effectiveFrom));
    }

    internal void Activate()
    {
        IsActive = true;
    }

    internal void Deactivate()
    {
        IsActive = false;
    }

    internal void ChangePriceHistory(Money newPrice, DateOnly effectiveFrom)
    {
        var existing = _priceHistory.FirstOrDefault(p => p.EffectiveFrom == effectiveFrom);
        if (existing != null)
        {
            _priceHistory.Remove(existing);
        }

        _priceHistory.Add(new PlanPriceHistory(newPrice, effectiveFrom));
    }
}