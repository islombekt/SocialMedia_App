using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Enities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DatabaseContextFactory _contextFactory;
        public PostRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task CreateAsync(PostEntity post)
        {
            try
            {
             using DatabaseContext context = _contextFactory.CreateDbContext();
             context.Posts.Add(post);
            _ = await context.SaveChangesAsync();
           }
           catch(Exception ex)
           {
            Console.WriteLine($"ERRROR on SAVE TO MSSQL {ex.Message}");
           }
            
        }
        
        public async Task DeleteAsync(Guid PostId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
           var post = await GetByIdAsync(PostId);

           if(post == null) return;
           //comments automatically deletes as we create entity with foreign key constraint for cascade(referencing child table records will alse be deleted) on delete
           context.Posts.Remove(post);
           _ = await context.SaveChangesAsync();
        }

        public async Task<PostEntity?> GetByIdAsync(Guid PostId)
        {
           using DatabaseContext context = _contextFactory.CreateDbContext();
           return await context.Posts
                               .Include(c=>c.Comments)
                               .FirstOrDefaultAsync(c => c.PostId == PostId);

        }

        public async Task<List<PostEntity>> ListAllAsync()
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking()
                               .Include(c=>c.Comments).AsNoTracking()
                               .ToListAsync();
        }

        public async Task<List<PostEntity>> ListByAuthorAsync(string author)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking()
                               .Include(c=>c.Comments)
                               .Where(c=> c.Author.Contains(author))
                               .AsNoTracking()
                               .ToListAsync();
        }

        public async Task<List<PostEntity>> ListByLikesAsync(int numberOfLikes)
        {
           using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking()
                               .Include(c=>c.Comments)
                               .Where(c=> c.Likes >= numberOfLikes)
                               .AsNoTracking()
                               .ToListAsync();
        }

        public async Task<List<PostEntity>> ListWithCommentsAsync()
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking()
                               .Include(c=>c.Comments)
                               .Where(c=> c.Comments != null && c.Comments.Any())
                               .AsNoTracking()
                               .ToListAsync();
        }

        public async Task UpdateAsync(PostEntity post)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.Posts.Update(post);
            _ = await context.SaveChangesAsync();
        }
    }
}