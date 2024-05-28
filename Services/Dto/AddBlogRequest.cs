using System.ComponentModel.DataAnnotations;
using Entities;

namespace Services.Dto
{
    public class AddBlogRequest
    {
        public required User Requestor { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public required string Content { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public required string Category { get; set; }
    }
}