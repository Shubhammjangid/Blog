using BlogManagement.Helper;
using Data.Repository;
using Data.Repository.Interface;
using Services;

namespace BlogManagement.DependencyInjection
{
    public class InjectDependency
    {
        public InjectDependency() { }
        public static void InjectDependencies(IServiceCollection services)
        {
            //Repository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<IBlogLikeReposiotry, BlogLikeReposiotry>();
            services.AddScoped<IBlogCommentRepository, BlogCommentRepository>();
           
            //services
            services.AddScoped<UserService>();
            services.AddScoped<BlogService>();
            services.AddScoped<CookieUserDetailsHandler>();
        }
    }
}