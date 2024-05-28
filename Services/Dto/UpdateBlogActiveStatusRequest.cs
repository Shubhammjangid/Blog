using Entities;

namespace Services.Dto
{
    public class UpdateBlogActiveStatusRequest
    {
        public User Requestor { get; set; }
        public int BlogId { get; set; }
        public bool IsActive { get; set; }
    }
}