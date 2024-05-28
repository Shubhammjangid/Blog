using Entities.Models;

namespace Data.Repository.Interface
{
    public interface IBlogLikeReposiotry : IBaseRepository
    {
        Task<BlogLike?> GetByBlogIdAndLikedById(int blogId, long likedById);
    }
}