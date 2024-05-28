using Microsoft.EntityFrameworkCore;
using Entities;
using Entities.Models;

namespace BlogManagement.Data;
public class BlogDbContext : DbContext
{
    public BlogDbContext()
        {
        }

        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Blog> Blog { get; set; }
        public DbSet<BlogLike> BlogLike { get; set; }
        public DbSet<BlogComment> BlogComment { get; set; }
}
