using CQRS.Core.Events;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Post.Common.Events
{
    public class CommentUpdatedEvent : BaseEvent
    {
        public CommentUpdatedEvent() : base(nameof(CommentUpdatedEvent)){

        }
       [BsonRepresentation(BsonType.String)] 
       public Guid CommentId{get;set;}
       public string Comment {get;set;}
       public string UserName {get;set;}
       public DateTime EditDate{get;set;}
    
    }
}