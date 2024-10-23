using System.ComponentModel.DataAnnotations;

namespace PostManagementRazorPages.Models
{
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty ;
    }
}
