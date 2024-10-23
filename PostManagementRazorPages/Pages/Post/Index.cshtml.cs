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
    public class IndexModel : PageModel
    {
        private readonly PostManagementRazorPages.Models.AppDbContext _context;

        public IndexModel(PostManagementRazorPages.Models.AppDbContext context)
        {
            _context = context;
        }

        public IList<PostManagementRazorPages.Models.Post> Post { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Post = await _context.Posts
                .Include(p => p.AppUser)
                .Include(p => p.Category).ToListAsync();
        }
    }
}
