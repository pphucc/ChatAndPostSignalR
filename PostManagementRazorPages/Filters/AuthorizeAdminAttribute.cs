using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PostManagementRazorPages.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAdminAttribute : Attribute, IAsyncPageFilter
    {
        public async Task OnPageHandlerExecutingAsync(PageHandlerExecutingContext context)
        {
            // Get the user email from the session
            var userEmail = context.HttpContext.Session.GetString("Email");
            // Check if the user is logged in and is an admin
            if (string.IsNullOrEmpty(userEmail) || userEmail != "admin@admin.com")
            {
                // Redirect to the Unauthorized page
                context.Result = new RedirectToPageResult("~/Unauthorized");
                return; // Ensure to return early if redirected
            }

            // If the user is authorized, proceed with the request
            await Task.CompletedTask; // This is a placeholder; you can remove it
        }

        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context) => Task.CompletedTask;

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next) => await next();
    }
}
