
using Entities;

namespace Services.Dto
{
    public class AddLikeToBlogRequest
    {
        public User Requestor { get; set; }
        public int BlogId { get; set; }
    }
}