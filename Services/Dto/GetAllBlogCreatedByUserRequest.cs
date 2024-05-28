using Entities;

namespace Services.Dto
{
    public class GetAllBlogCreatedByUserRequest
    {
        public User Requestor { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string SearchTerm { get; set; }
    }
}