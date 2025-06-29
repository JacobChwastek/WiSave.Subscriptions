using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using WiSave.Shared.Types;

namespace WiSave.Subscriptions.Projections.Models;

public class SubscriptionPlan
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = null!;

    [BsonElement("isActive")]
    public bool IsActive { get; set; }

    [BsonElement("effectiveFrom")]
    [BsonSerializer(typeof(DateOnlySerializer))]
    public DateOnly EffectiveFrom { get; set; }

    [BsonElement("priceHistory")]
    public List<PlanPriceHistory> PriceHistory { get; set; } = [];

    [BsonIgnore]
    public Money CurrentPrice => PriceHistory
        .Where(p => p.EffectiveFrom <= DateOnly.FromDateTime(DateTime.UtcNow))
        .OrderByDescending(p => p.EffectiveFrom)
        .FirstOrDefault()?.Price ?? Money.Zero(Currency.USD);
}