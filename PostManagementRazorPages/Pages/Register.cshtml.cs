using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PostManagementRazorPages.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace PostManagementRazorPages.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public RegisterViewModel Input { get; set; } = new RegisterViewModel();

        private readonly AppDbContext _context;

        public RegisterModel(AppDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Check if the email already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == Input.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Email already exists.");
                return Page();
            }

            var newUser = new PostManagementRazorPages.Models.AppUser
            {
                UserId = Guid.NewGuid(),
                FullName = Input.FullName,
                Address = Input.Address,
                Email = Input.Email,
                Password = HashPassword(Input.Password) // Hash the password here
            };

            // Handle profile picture upload
            if (Input.ProfilePicture != null)
            {
                var uploadsFolder = Path.Combine("wwwroot", "uploads", "profile-pictures");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Input.ProfilePicture.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.ProfilePicture.CopyToAsync(fileStream);
                }

                newUser.ProfilePicture = "/uploads/profile-pictures/" + uniqueFileName;
            }
            else
            {
                newUser.ProfilePicture = "/uploads/profile-pictures/default-avatar.jpg"; // Optional: Set a default image path
            }

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return RedirectToPage("Login");
        }

        // Method to hash the password
        private string HashPassword(string password)
        {
            using (var hmac = new HMACSHA256())
            {
                var salt = Convert.ToBase64String(hmac.Key);
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return salt + ":" + Convert.ToBase64String(hash);
            }
        }
    }

    // ViewModel for validation
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }

        [DataType(DataType.Upload)]
        [AllowNull]
        public IFormFile? ProfilePicture { get; set; }
    }
}
