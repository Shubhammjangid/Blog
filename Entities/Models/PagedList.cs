namespace Entities.Models
{
    public class PagedList<T>
    {
        public ICollection<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public User Requestor { get; set; }
    }
}