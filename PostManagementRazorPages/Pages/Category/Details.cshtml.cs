using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PostManagementRazorPages.Models;

namespace PostManagementRazorPages.Pages.Category
{
    public class DetailsModel : PageModel
    {
        private readonly PostManagementRazorPages.Models.AppDbContext _context;

        public DetailsModel(PostManagementRazorPages.Models.AppDbContext context)
        {
            _context = context;
        }

        public PostManagementRazorPages.Models.Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }
            else
            {
                Category = category;
            }
            return Page();
        }
    }
}
