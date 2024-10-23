using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using PostManagementRazorPages.Hubs;
using PostManagementRazorPages.Models;

namespace PostManagementRazorPages.Pages.Post
{
    public class CreateModel : PageModel
    {
        private readonly PostManagementRazorPages.Models.AppDbContext _context;
        private readonly IHubContext<PostHub> _postHub;

        public CreateModel(PostManagementRazorPages.Models.AppDbContext context, IHubContext<PostHub> postHub)
        {
            _context = context;
            _postHub = postHub;
        }

        public IActionResult OnGet()
        {
        ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
        ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId");
            return Page();
        }

        [BindProperty]
        public PostManagementRazorPages.Models.Post Post { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Posts.Add(Post);
            await _context.SaveChangesAsync();
            await _postHub.Clients.All.SendAsync("ReceivePostUpdate", "A new post was created!");

            return RedirectToPage("./Index");
        }
    }
}
