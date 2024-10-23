using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PostManagementRazorPages.Filters;
using PostManagementRazorPages.Models;

namespace PostManagementRazorPages.Pages.AppUser
{

    [AuthorizeAdmin]
    public class CreateModel : PageModel
    {
        private readonly PostManagementRazorPages.Models.AppDbContext _context;

        public CreateModel(PostManagementRazorPages.Models.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public PostManagementRazorPages.Models.AppUser AppUser { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Users.Add(AppUser);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
