using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace PostManagementRazorPages.Hubs
{
    public class PostHub : Hub
    {
        // This method will broadcast post changes to all connected clients
        public async Task SendPostUpdate(string message)
        {
            await Clients.All.SendAsync("ReceivePostUpdate", message);
        }
    }
}
