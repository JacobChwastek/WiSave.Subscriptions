using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace WiSave.Subscriptions.Projections.Database.Serializers;

public class NullableDateOnlySerializer : SerializerBase<DateOnly?>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateOnly? value)
    {
        if (value.HasValue)
            context.Writer.WriteString(value.Value.ToString("yyyy-MM-dd"));
        else
            context.Writer.WriteNull();
    }
    
    public override DateOnly? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        switch (context.Reader.CurrentBsonType)
        {
            case BsonType.Null:
                context.Reader.ReadNull();
                return null;
            case BsonType.String:
                return DateOnly.Parse(context.Reader.ReadString());
            default:
                throw new BsonSerializationException("Cannot deserialize DateOnly?");
        }
    }
}