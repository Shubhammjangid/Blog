namespace Entities.Models
{
    public class BlogLike
    {
        public BlogLike(){ }
        public BlogLike(int blogId, long likedById)
        {
            BlogId = blogId;
            LikedById = likedById;
            IsDeleted = false;
            CreatedAt = DateTime.UtcNow;
        }
        public long Id { get; set; }
        public int BlogId { get; set; }
        public long LikedById { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public User LikedBy { get; set; }
        public Blog Blog { get; set; }
    }
}