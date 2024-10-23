using System.ComponentModel.DataAnnotations;

namespace PostManagementRazorPages.Models
{
    public class AppUser
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Url]
        public string ProfilePicture { get; set; } = string.Empty;  // New property to store the image URL
    }
}
