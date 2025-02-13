using CQRS.Core.Commands;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Post.Cmd.Api.Commands
{
    public class EditCommentCommand : BaseCommand
    {
        [BsonRepresentation(BsonType.String)]
        public Guid CommentId {get;set;}
        public string Comment {get;set;}
        public string UserName {get;set;}
    }
}