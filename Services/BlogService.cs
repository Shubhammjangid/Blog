using Data.Repository.Interface;
using Entities;
using Entities.Models;
using Services.Dto;
using Services.Dto.Admin;

namespace Services
{
    public class BlogService
    {
        private readonly IBlogRepository blogRepository;
        private readonly IBlogLikeReposiotry blogLikeReposiotry;
        private readonly IBlogCommentRepository blogCommentRepository;
        public BlogService(IBlogRepository blogRepository, IBlogLikeReposiotry blogLikeReposiotry, IBlogCommentRepository blogCommentRepository)
        {
            this.blogRepository = blogRepository;
            this.blogLikeReposiotry = blogLikeReposiotry;
            this.blogCommentRepository = blogCommentRepository;
        }

        public async Task<(Blog?, string)> Add(AddBlogRequest dto)
        {
            if(dto.Requestor is null || dto.Requestor.Role.Name == Constants.Keys.ADMIN)
            {
                return (null, Constants.Keys.FORBBIDEN);
            }

            if(string.IsNullOrWhiteSpace(dto.Content) || string.IsNullOrWhiteSpace(dto.Title) || string.IsNullOrWhiteSpace(dto.Category))
            {
                return (null, Constants.Keys.INVALID_DATA);
            }

            var blog = new Blog(dto.Title, dto.Content, dto.Category, dto.Requestor);
            
            blogRepository.Add(blog);

            await blogRepository.SaveAsync();

            return (blog, Constants.Blog.BLOG_ADDED);
        }

        public async Task<(ICollection<Blog>, int)> GetBlogs(GetAllBlogCreatedByUserRequest dto)
        {
            if(dto.PageNo <= 0)
            {
                dto.PageNo = 1;
            }

            if(dto.PageSize <= 0)
            {
                dto.PageSize = 4;
            }
            var (blogs, totalCount) = await blogRepository.GetAllByCreatedById(dto.PageNo, dto.PageSize, dto.SearchTerm, dto.Requestor.Id);
            return (blogs, totalCount);
        }

        public async Task<(ICollection<Blog>, int)> GetAllBlogs(GetAllBlogCreatedByUserRequest dto)
        {
            if(dto.PageNo <= 0)
            {
                dto.PageNo = 1;
            }

            if(dto.PageSize <= 0)
            {
                dto.PageSize = 4;
            }
            var (blogs, totalCount) = await blogRepository.GetAll(dto.PageNo, dto.PageSize, dto.SearchTerm);
            return (blogs, totalCount);
        }

        public async Task<(Blog?, string)> Approve(ApproveBlogRequest dto)
        {
            if(dto.Requestor.Role.Name != Constants.Keys.ADMIN)
            {
                return (null, Constants.Keys.FORBBIDEN);
            }

            var blog = await blogRepository.GetById(dto.BlogId);
            if(blog is null)
            {
                return (blog, Constants.Blog.BLOG_DOES_NOT_EXISTS);
            }

            dto.ReasonForNotApproval = Constants.Blog.DIS_APPROVED;

            if(!dto.IsApprovedByAdmin && string.IsNullOrWhiteSpace(dto.ReasonForNotApproval))
            {
                return (null, Constants.Blog.ADD_REASON_FOR_NOT_APPROVAL);
            }

            blog.ApproveBlog(dto.IsApprovedByAdmin, dto.ReasonForNotApproval);       

            blogRepository.Update(blog); 

            await blogRepository.SaveAsync();

            return (blog, dto.IsApprovedByAdmin ? Constants.Blog.BLOG_APPROVED_SUCCESSFULLY : Constants.Blog.BLOG_DIS_APPROVED_SUCCESSFULLY);    
        }

        public async Task<(Blog?, string)> UpdateActiveStatus(UpdateBlogActiveStatusRequest dto)
        {
            var blog = await blogRepository.GetById(dto.BlogId);
            if(blog is null)
            {
                return (blog, Constants.Blog.BLOG_DOES_NOT_EXISTS);
            }

            if(dto.Requestor.Id != blog.CreatedById)
            {
                return (null, Constants.Keys.FORBBIDEN);
            }

            blog.UpdateActiveFlag(!blog.IsActive);

            blogRepository.Update(blog);

            await blogRepository.SaveAsync();

            return (blog, Constants.Blog.BLOG_ACTIVE_STATUS_UPDATED);
        }

        public async Task<(ICollection<Blog>, int)> GetAllBlogCretedByOther(GetAllBlogCreatedByUserRequest dto)
        {
            if(dto.PageNo <= 0)
            {
                dto.PageNo = 1;
            }

            if(dto.PageSize <= 0)
            {
                dto.PageSize = 4;
            }
            var (blogs, totalCount) = await blogRepository.GetAllBlogs(dto.PageNo, dto.PageSize, dto.SearchTerm, dto.Requestor.Id);
           
            return (blogs, totalCount);
        }

        public async Task<(Blog?, string)> AddLike(AddLikeToBlogRequest dto)
        {
            var blog = await blogRepository.GetById(dto.BlogId);
            if(blog is null)
            {
                return (blog, Constants.Blog.BLOG_DOES_NOT_EXISTS);
            }

            if(dto.Requestor.Id == blog.CreatedById)
            {
                return (null, Constants.Keys.FORBBIDEN);
            }

            if(dto.Requestor.Role.Name == Constants.Keys.ADMIN)
            {
                return (null, Constants.Keys.FORBBIDEN);
            }

            var like = await blogLikeReposiotry.GetByBlogIdAndLikedById(blog.Id, dto.Requestor.Id);
            if(like is null)
            {
                var blogLike = new BlogLike(blog.Id, dto.Requestor.Id);
            
                blog.AddLike();

                blogLikeReposiotry.Add(blogLike);

                await blogLikeReposiotry.SaveAsync();

                return (blog, "Like added successfully");
            }

            blogLikeReposiotry.Delete(like);

            blog.RemoveLike();

            await blogLikeReposiotry.SaveAsync();

            return (blog, "You have already liked this blog.");
        }
        public async Task<Blog?> GetById(int id)
        {
            return await blogRepository.GetById(id);
        }

        public async Task<(Blog?, string)> Update(Blog dto)
        {
            var blog = await GetById(dto.Id);
            if(blog is null)
            {
                return (blog, Constants.Blog.BLOG_DOES_NOT_EXISTS);
            }

            blog.Update(dto.Title, dto.Content, dto.Category);

            blogRepository.Update(blog);

            await blogRepository.SaveAsync();

            return (blog, "Blog updated successfully");
        }

        public async Task<(Blog?, string)> AddComment(AddCommentRequest dto)
        {
            if(string.IsNullOrWhiteSpace(dto.comment))
            {
                return (null, Constants.Keys.INVALID_DATA);
            }

            var blog = await blogRepository.GetById(dto.BlogId);
            if(blog is null)
            {
                return (blog, Constants.Blog.BLOG_DOES_NOT_EXISTS);
            }

            if(dto.Requestor.Id == blog.CreatedById || dto.Requestor.Role.Name == Constants.Keys.ADMIN)
            {
                return (null, Constants.Keys.FORBBIDEN);
            }

            var commentByRequestor = await blogCommentRepository.GetByBlogIdAndCommentById(dto.BlogId, dto.Requestor.Id);
            if(commentByRequestor.Count >= 5)
            {
                return (null, Constants.BLOG_COMMENT.LIMIT_REACHED);
            }

            if(dto.comment.Length >= 300)
            {
                return (null, Constants.BLOG_COMMENT.MAX_LENGTH);
            }

            var comment = new BlogComment(dto.comment, dto.BlogId, dto.Requestor.Id);

            blogRepository.Add(comment);

            await blogRepository.SaveAsync();

            return (blog, Constants.BLOG_COMMENT.ADDED_SUCCESSFULLY);
        }
    }
}