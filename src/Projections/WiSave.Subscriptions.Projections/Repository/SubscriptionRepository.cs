using MongoDB.Bson;
using MongoDB.Driver;
using WiSave.Subscriptions.Projections.Models;

namespace WiSave.Subscriptions.Projections.Repository;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly IMongoCollection<Subscription> _collection;

    public SubscriptionRepository(IMongoDatabase database)
    {
        const string collectionName = "subscriptions";
        
        var collectionNames = database.ListCollectionNames().ToList();
        if (!collectionNames.Contains(collectionName))
        {
            database.CreateCollection(collectionName);
        }

        _collection = database.GetCollection<Subscription>(collectionName);
    }

    public async Task<Subscription?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Subscription>.Filter.Eq(x => x.Id, id);
        return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Subscription>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _collection.Find(FilterDefinition<Subscription>.Empty).ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Subscription model, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Subscription>.Filter.Eq(x => x.Id, model.Id);
        await _collection.ReplaceOneAsync(filter, model, new ReplaceOptions { IsUpsert = false }, cancellationToken);
    }

    public async Task<(List<Subscription> Items, long TotalCount)> GetByUserIdAsync(string userId, string? name,
        string? plan,
        bool? isTrial,
        int skip,
        int take,
        CancellationToken cancellationToken = default)
    {
        var filters = new List<FilterDefinition<Subscription>>();
        
        filters.Add(Builders<Subscription>.Filter.Eq(x => x.UserId, Guid.Parse(userId)));
        
        if (!string.IsNullOrWhiteSpace(name))
            filters.Add(Builders<Subscription>.Filter.Regex(x => x.Name, new BsonRegularExpression(name, "i")));

        if (!string.IsNullOrWhiteSpace(plan))
            filters.Add(Builders<Subscription>.Filter.Eq(x => x.Plan, plan));

        if (isTrial.HasValue)
            filters.Add(Builders<Subscription>.Filter.Eq(x => x.IsTrial, isTrial));

        var filter = filters.Count != 0 ? Builders<Subscription>.Filter.And(filters) : FilterDefinition<Subscription>.Empty;

        var totalCount = await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var items = await _collection
            .Find(filter)
            .Skip(skip)
            .Limit(take)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task InsertAsync(Subscription model, CancellationToken cancellationToken = default)
    {
        await _collection.InsertOneAsync(model, cancellationToken: cancellationToken);
    }
}