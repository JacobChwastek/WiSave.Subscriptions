using WiSave.Shared.Types;

namespace WiSave.Subscriptions.Contracts.Dtos;

public record PlanPriceHistoryDto(
    Money Price,
    DateOnly EffectiveFrom
);