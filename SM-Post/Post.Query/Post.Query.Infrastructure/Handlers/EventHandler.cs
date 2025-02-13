using Post.Common.Events;
using Post.Query.Domain.Enities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Infrastructure.Handlers
{
    public class EventHandler : IEventHandler
    {
        private readonly IPostRepository _postRepo;
        private readonly ICommentRepository _commentRepo;
        public EventHandler( IPostRepository postRepo, ICommentRepository commentRepository)
        {
            _commentRepo   = commentRepository;
            _postRepo = postRepo;
        }
        public async Task On(PostCreatedEvent @event)
        {
           var post = new PostEntity
           {
            PostId = @event.Id,
            Author = @event.Author,
            DatePosted = @event.DatePosted,
            Message = @event.Message
           };
           await _postRepo.CreateAsync(post);
        }

        public async Task On(MessageUpdatedEvent @event)
        {
            try
            {
            var post = await _postRepo.GetByIdAsync(@event.Id);
            if(post == null) return;
            post.Message = @event.Message;
            await _postRepo.UpdateAsync(post);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }

        public async Task On(PostLikedEvent @event)
        {
             var post = await _postRepo.GetByIdAsync(@event.Id);
            if(post == null) return;
            post.Likes++;
            await _postRepo.UpdateAsync(post);

        }

        public async Task On(CommentAddedEvent @event)
        {
            var comment = new CommentEntity
            {
                PostId = @event.Id,
                CommentId = @event.CommentId,
                CommentDate = @event.CommentDate,
                Comment = @event.Comment,
                Username = @event.UserName,
                Edited =false
            };
            await _commentRepo.CreateAsync(comment);
        }

        public async Task On(CommentUpdatedEvent @event)
        {
            var comment = await _commentRepo.GetByIdAsync(@event.CommentId);
            if(comment == null) return;
            comment.Comment = @event.Comment;
            comment.Edited = true;
            comment.CommentDate = @event.EditDate;

            await _commentRepo.UpdateAsync(comment);
        }

        public  async Task On(CommentRemovedEvent @event)
        {
            await _commentRepo.DeleteAsync(@event.CommentId);
        }

        public async Task On(PostRemovedEvent @event)
        {
           await _postRepo.DeleteAsync(@event.Id);
        }
    }
}