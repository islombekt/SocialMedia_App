using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Post.Query.Domain.Enities
{
     [Table("Comment")]
    public class CommentEntity
    {
         [Key]
         [BsonRepresentation(BsonType.String)] 
        public Guid CommentId {get;set;}
        public string Username{get;set;}
        public DateTime CommentDate {get;set;}
        public string Comment {get;set;}
        public bool Edited {get;set;}
        public Guid PostId{get;set;}
        
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual PostEntity Post {get;set;}
    }
}