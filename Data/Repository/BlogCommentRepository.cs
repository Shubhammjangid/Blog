using BlogManagement.Data;
using Data.Repository.Interface;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class BlogCommentRepository : BaseRepository, IBlogCommentRepository
    {
        private readonly BlogDbContext context;
        public BlogCommentRepository(BlogDbContext context): base(context)
        {
            this.context = context;
        }

        public async Task<ICollection<BlogComment>> GetByBlogIdAndCommentById(int blogId, long commentById)
        {
            return await context.BlogComment.Where(it => it.BlogId == blogId && it.CommentById == commentById).ToListAsync();
        }
    }
}