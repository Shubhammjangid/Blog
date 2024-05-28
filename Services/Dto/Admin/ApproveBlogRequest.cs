using Entities;

namespace Services.Dto.Admin
{
    public class ApproveBlogRequest
    {
        public User Requestor { get; set; }
        public int BlogId { get; set; }
        public bool IsApprovedByAdmin { get; set; }
        public string ReasonForNotApproval { get; set; }
    }
}