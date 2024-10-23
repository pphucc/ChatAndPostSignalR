using Microsoft.EntityFrameworkCore;
using PostManagementRazorPages.Hubs;
using PostManagementRazorPages.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout duration
    options.Cookie.HttpOnly = true; // Make the cookie HTTP-only
    options.Cookie.IsEssential = true; // Make the session cookie essential for the app
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();


app.UseAuthorization();
app.MapGet("/", (context) =>
{
    context.Response.Redirect("/Login"); // Adjust the path to your actual login page
    return Task.CompletedTask;
});
app.MapRazorPages();
app.MapHub<PostHub>("/postHub");
app.MapHub<ChatHub>("/chatHub");

app.Run();
