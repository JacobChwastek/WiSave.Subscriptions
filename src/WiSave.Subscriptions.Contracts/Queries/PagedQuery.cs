namespace WiSave.Subscriptions.Contracts.Queries;

public record PagedQuery(
    int PageNumber = 1,
    int PageSize = 20
);