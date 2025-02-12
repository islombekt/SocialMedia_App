using MongoDB.Bson;

namespace CQRS.Core.Messages
{
    public abstract class Message
    {
        [MongoDB.Bson.Serialization.Attributes.BsonRepresentation(BsonType.String)]
        public Guid Id {get;set;}

    }
}