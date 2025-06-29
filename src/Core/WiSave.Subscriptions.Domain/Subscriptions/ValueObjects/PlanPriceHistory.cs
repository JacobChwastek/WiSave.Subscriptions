using WiSave.Shared.Types;

namespace WiSave.Subscriptions.Domain.Subscriptions.ValueObjects;

public record PlanPriceHistory(Money Price, DateOnly EffectiveFrom);