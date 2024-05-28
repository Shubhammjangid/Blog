using Entities.Models;

namespace Data.Repository.Interface
{
    public interface IBlogCommentRepository : IBaseRepository
    {
        Task<ICollection<BlogComment>> GetByBlogIdAndCommentById(int blogId, long commentById);
    }
}