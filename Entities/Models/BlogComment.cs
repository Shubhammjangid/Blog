namespace Entities.Models
{
    public class BlogComment
    {
        public BlogComment() {}

        public BlogComment(string comment, int blogId, long commentById)
        {
            Comment = comment;
            BlogId = blogId;
            CommentById = commentById;
            CreatedAt = DateTime.UtcNow;
        }
        public long Id { get; set; }
        public string Comment { get; set; }
        public int BlogId { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CommentById { get; set; }
        public User CommentBy { get; set; }
        public Blog Blog { get; set; }
    }
}