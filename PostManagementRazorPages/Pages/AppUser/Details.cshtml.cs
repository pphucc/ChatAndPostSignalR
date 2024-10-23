using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PostManagementRazorPages.Models;

namespace PostManagementRazorPages.Pages.AppUser
{
    public class DetailsModel : PageModel
    {
        private readonly PostManagementRazorPages.Models.AppDbContext _context;

        public DetailsModel(PostManagementRazorPages.Models.AppDbContext context)
        {
            _context = context;
        }

        public PostManagementRazorPages.Models.AppUser AppUser { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appuser = await _context.Users.FirstOrDefaultAsync(m => m.UserId == id);
            if (appuser == null)
            {
                return NotFound();
            }
            else
            {
                AppUser = appuser;
            }
            return Page();
        }
    }
}
