using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PostManagementRazorPages.Models;

namespace PostManagementRazorPages.Pages.Post
{
    public class DetailsModel : PageModel
    {
        private readonly PostManagementRazorPages.Models.AppDbContext _context;

        public DetailsModel(PostManagementRazorPages.Models.AppDbContext context)
        {
            _context = context;
        }

        public PostManagementRazorPages.Models.Post Post { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.Include("AppUser").Include("Category").FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }
            else
            {
                Post = post;
            }
            return Page();
        }
    }
}
