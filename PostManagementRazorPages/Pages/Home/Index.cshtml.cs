using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PostManagementRazorPages.Hubs;
using PostManagementRazorPages.Models;

namespace PostManagementRazorPages.Pages.Home
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<PostHub> _postHub;


        public IndexModel(AppDbContext context, IHubContext<PostHub> postHub)
        {
            _context = context;
            _postHub = postHub;
        }

        public IList<PostManagementRazorPages.Models.Post> Posts { get; set; }
        public int PageSize { get; set; } = 5; // Number of posts per page
        public int CurrentPage { get; set; }

        public async Task<IActionResult> OnGetAsync(int pageNumber = 1)
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToPage("/Login");
            }

            CurrentPage = pageNumber;

            Posts = await _context.Posts.Include(p => p.AppUser)
                                        .Include(p => p.Category)
                                        .OrderByDescending(p => p.CreatedDate)
                                        .Skip((pageNumber - 1) * PageSize)
                                        .Take(PageSize)
                                        .ToListAsync();

            return Page(); // Return the current page if no redirect is needed
        }
        public async Task<IActionResult> OnPostCreatePostAsync(string title, string content)
        {
            var userId = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("FullName");
            if (userId == null) return Unauthorized();

            var post = new Models.Post
            {
                Title = title,
                Content = content,
                UserId = Guid.Parse(userId),
                CreatedDate = DateTime.Now
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            // Notify all connected clients about the new post
            await _postHub.Clients.All.SendAsync("ReceivePostUpdate", $"{userName} created a new post");

            return RedirectToPage("/Home/Index");
        }
        public async Task<IActionResult> OnPostEditPostAsync(Guid postId, string title, string content)
        {
            var post = _context.Posts.FirstOrDefault(p => p.PostId == postId);
            var userId = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("FullName");
            if (post == null || post.UserId != Guid.Parse(HttpContext.Session.GetString("UserId")))
            {
                return NotFound();
            }

            post.Title = title;
            post.Content = content;
            post.UpdatedDate = DateTime.Now;
            _context.Update(post);
            await _context.SaveChangesAsync();

            // Notify all connected clients about the updated post
            await _postHub.Clients.All.SendAsync("ReceivePostUpdate", $"{userName} edited a post");

            return RedirectToPage("/Home/Index");
        }

        public async Task<IActionResult> OnPostDeletePostAsync(Guid postId)
        {
            var post = _context.Posts.FirstOrDefault(p => p.PostId == postId);
            var userId = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("FullName");
            if (post == null || post.UserId != Guid.Parse(HttpContext.Session.GetString("UserId")))
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            // Notify all connected clients about the deleted post
            await _postHub.Clients.All.SendAsync("ReceivePostUpdate", $"{userName} deleted a post");

            return RedirectToPage("/Home/Index");
        }
    }

}
