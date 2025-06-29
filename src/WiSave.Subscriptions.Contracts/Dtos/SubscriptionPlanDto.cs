using WiSave.Shared.Types;

namespace WiSave.Subscriptions.Contracts.Dtos;

public record SubscriptionPlanDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public bool IsActive { get; init; }
    public DateOnly EffectiveFrom { get; init; }
    public Money CurrentPrice { get; init; } = null!;
    public List<PlanPriceHistoryDto> PriceHistory { get; init; } = [];
    
    public SubscriptionPlanDto() { }
    
    public SubscriptionPlanDto(
        Guid id,
        string name,
        bool isActive,
        DateOnly effectiveFrom,
        Money currentPrice,
        List<PlanPriceHistoryDto> priceHistory)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        EffectiveFrom = effectiveFrom;
        CurrentPrice = currentPrice;
        PriceHistory = priceHistory;
    }
}