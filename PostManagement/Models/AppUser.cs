using System.ComponentModel.DataAnnotations;

namespace PostManagement.Models
{
    public class AppUser
    {
        [Key]
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty ;
        public string Email { get; set; } = string.Empty ;
        public string Password { get; set; } = string.Empty ;
    }
}
