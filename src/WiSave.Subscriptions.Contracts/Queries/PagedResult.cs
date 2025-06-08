namespace WiSave.Subscriptions.Contracts.Queries;

public record PagedResult<T>(
    List<T> Items,
    long TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages,
    bool HasNextPage,
    bool HasPreviousPage
)
{
    public static PagedResult<T> Create(List<T> items, long totalCount, int pageNumber, int pageSize)
    {
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResult<T>(
            Items: items,
            TotalCount: totalCount,
            PageNumber: pageNumber,
            PageSize: pageSize,
            TotalPages: totalPages,
            HasNextPage: pageNumber < totalPages,
            HasPreviousPage: pageNumber > 1
        );
    }
}