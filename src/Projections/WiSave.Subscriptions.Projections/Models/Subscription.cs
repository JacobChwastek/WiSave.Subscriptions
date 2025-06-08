using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using WiSave.Shared.Types;

namespace WiSave.Subscriptions.Projections.Models;

public class Subscription
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = null!;

    [BsonElement("plan")]
    public string Plan { get; set; } = null!;

    [BsonElement("money")]
    public Money Money { get; set; } = null!;

    [BsonElement("periodUnit")]
    [BsonRepresentation(BsonType.String)]
    public PeriodUnit PeriodUnit { get; set; }

    [BsonElement("periodInterval")]
    public int PeriodInterval { get; set; }

    [BsonElement("autoRenew")]
    public bool AutoRenew { get; set; }

    [BsonElement("startDate")]
    [BsonSerializer(typeof(DateOnlySerializer))]
    public DateOnly StartDate { get; set; }

    [BsonElement("isTrial")]
    public bool IsTrial { get; set; }

    [BsonElement("maxRenewals")]
    public int? MaxRenewals { get; set; }

    [BsonElement("trialDurationInDays")]
    public int? TrialDurationInDays { get; set; }
}
