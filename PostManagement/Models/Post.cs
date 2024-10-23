using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using PostManagement.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostManagement.Models
{
    public class Post
    {
        [Key]
        public Guid PostId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public PublishStatus PublishStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid? UserId { get; set; }
        public Guid? CategoryId { get; set; }

        [ForeignKey(nameof(UserId))]
        [ValidateNever]
        public AppUser AppUser { get; set; }

        [ForeignKey(nameof(CategoryId))]
        [ValidateNever]
        public Category Category { get; set; }
    }
}
