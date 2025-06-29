using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using WiSave.Shared.Types;

namespace WiSave.Subscriptions.Projections.Models;

public class PlanPriceHistory
{
    [BsonElement("price")]
    public Money Price { get; set; } = null!;

    [BsonElement("effectiveFrom")]
    [BsonSerializer(typeof(DateOnlySerializer))]
    public DateOnly EffectiveFrom { get; set; }
}