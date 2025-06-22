using WiSave.Subscriptions.Projections.Models;

namespace WiSave.Subscriptions.Projections.Repository;

public interface ISubscriptionRepository
{
    Task<Subscription?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Subscription>> GetAllAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(Subscription model, CancellationToken cancellationToken = default);
    Task<(List<Subscription> Items, long TotalCount)> GetByUserIdAsync(string userId, string? name,
        string? plan,
        bool? isTrial,
        int skip,
        int take,
        CancellationToken cancellationToken = default);
    Task InsertAsync(Subscription model, CancellationToken cancellationToken = default);
}