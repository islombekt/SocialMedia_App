using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Post.Query.Domain.Enities;

namespace Post.Query.Domain.Repositories
{
    public interface IPostRepository
    {
        Task CreateAsync(PostEntity post);
        Task UpdateAsync(PostEntity post);
        Task DeleteAsync(Guid PostId);
        Task<PostEntity> GetByIdAsync(Guid PostId);
        Task<List<PostEntity>> ListAllAsync();
        Task<List<PostEntity>> ListByAuthorAsync(string author);
        Task<List<PostEntity>> ListByLikesAsync(int numberOfLikes);
        Task<List<PostEntity>> ListWithCommentsAsync();
    }
}