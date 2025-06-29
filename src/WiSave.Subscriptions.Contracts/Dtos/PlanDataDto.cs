using WiSave.Shared.Types;

namespace WiSave.Subscriptions.Contracts.Dtos;

public sealed record PlanDataDto(
    Guid Id,
    string Name,
    Money Price
);