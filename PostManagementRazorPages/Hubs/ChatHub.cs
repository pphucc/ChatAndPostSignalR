using Microsoft.AspNetCore.SignalR;

namespace PostManagementRazorPages.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string userId, string userProfile, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", userId, userProfile, message);
        }
    }
}
