using BlogManagement.Data;
using Data.Repository.Interface;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class BlogLikeReposiotry : BaseRepository, IBlogLikeReposiotry
    {
        private readonly BlogDbContext context;
        public BlogLikeReposiotry(BlogDbContext context): base(context)
        {
            this.context = context;
        }

        public async Task<BlogLike?> GetByBlogIdAndLikedById(int blogId, long likedById)
        {
            return await context.BlogLike.FirstOrDefaultAsync(it => it.BlogId == blogId && it.LikedById == likedById);
        }
    }
}