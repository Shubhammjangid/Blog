using Entities;

namespace Services.Dto
{
    public class AddCommentRequest
    {
        public User Requestor { get; set; }
        public int BlogId { get; set; }
        public string comment { get; set; }
    }
}