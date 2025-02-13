using CQRS.Core.Events;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Post.Common.Events
{
    public class CommentAddedEvent : BaseEvent
    {
        public CommentAddedEvent() : base(nameof(CommentAddedEvent)){

        }
        [BsonRepresentation(BsonType.String)]
       public Guid CommentId{get;set;}
       public string Comment {get;set;}
       public string UserName {get;set;}
       public DateTime CommentDate{get;set;}
    
    }
}