namespace Services.Dto
{
    public class UpdateBlogRequest
    {
        public int BlogId { get; set; }
        public  string Title { get; set; }
        public  string Content { get; set; }
        public  string Category { get; set; }
    }
}