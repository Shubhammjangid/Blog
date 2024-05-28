namespace Entities.Models
{
    public class Blog
    {
        public Blog() {}

        public Blog(string title, string content, string category, User createdBy)
        {
            Title = title;
            Content = content;
            Author = createdBy.FirstName + " " + createdBy.LastName;
            CreatedDate = DateTime.UtcNow;
            Category = category;
            Views = 0;
            CreatedById = createdBy.Id;
            IsActive = true;
        }
        public int Id { get; set; }
        public  string Title { get; set; }
        public  string Content { get; set; }
        public  string Category { get; set; }
        public  string Author { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int Views { get; set; }
        public long CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsApprovedByAdmin { get; set; }
        public string? ReasonForNotApproval { get; set; }
        public int Likes { get; set; }
        public ICollection<BlogLike> BlogLike { get; set; }
        public ICollection<BlogComment> BlogComment { get; set; }

        public void Update(string title, string content, string category)
        {
            Title = title;
            Content = content;
            Category = category;
            LastModifiedDate = DateTime.UtcNow;
        }

        public void ApproveBlog(bool isApprovedByAdmin, string reasonForNotApproval)
        {
            IsApprovedByAdmin = isApprovedByAdmin;
            if(!isApprovedByAdmin)
            {
                ReasonForNotApproval = reasonForNotApproval;
            }
        }

        public void UpdateActiveFlag(bool isActive)
        {
            IsActive = isActive;
            LastModifiedDate = DateTime.UtcNow;
        }

        public void AddLike()
        {
            Likes += 1;
        }
        public void RemoveLike()
        {
            Likes -= 1;
        }
    }
}