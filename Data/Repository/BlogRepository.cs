
using BlogManagement.Data;
using Data.Repository.Interface;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class BlogRepository : BaseRepository, IBlogRepository
    {
        private readonly BlogDbContext context;
        public BlogRepository(BlogDbContext context): base(context)
        {
            this.context = context;
        }

        public async Task<(ICollection<Blog>, int)> GetAllByCreatedById(int pageNo, int pageSize, string? searchTerm, long requestorId)
        {
            IQueryable<Blog> query = context.Blog
                .Include(it => it.BlogLike)
                    .ThenInclude(it => it.LikedBy)
                .Include(it => it.BlogComment)
                .Where(it => it.CreatedById == requestorId).OrderByDescending(it => it.Id);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(it => it.Title.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var blogs = await query.Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            return (blogs, totalCount);
        }
        
        public async Task<(ICollection<Blog>, int)> GetAll(int pageNo, int pageSize, string? searchTerm)
        {
            IQueryable<Blog> query = context.Blog 
                .Include(it => it.BlogLike)
                    .ThenInclude(it => it.LikedBy)
                .Include(it => it.BlogComment);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(it => it.Title.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var blogs = await query.Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            return (blogs, totalCount);
        }

        public async Task<Blog?> GetById(int id)
        {
            return await context.Blog
            .Include(it => it.BlogLike)
                .ThenInclude(it => it.LikedBy)
            .Include(it => it.BlogComment)
                .ThenInclude(it => it.CommentBy)
            .FirstOrDefaultAsync(it => it.Id == id);
        }

        public async Task<(ICollection<Blog>, int)> GetAllBlogs(int pageNo, int pageSize, string? searchTerm, long requestorId)
        {
            IQueryable<Blog> query = context.Blog
            .Include(it => it.BlogLike)
                .ThenInclude(it => it.LikedBy)
            .Include(it => it.BlogComment)
            .Where(e => e.CreatedById != requestorId && e.IsActive && e.IsApprovedByAdmin);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(it => it.Title.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var blogs = await query.Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            return (blogs, totalCount);
        }
    }
}