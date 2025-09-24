using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace FitFlex.Chatting
{
    //[Authorize]
    public class ChatHub : Hub
    {
        public async Task SendMessageToUser(string receiverId, string message)
        {
            await Clients.User(receiverId).SendAsync("ReceiveMessage", Context.UserIdentifier, message);
        }

        public async Task SendMessageToAll(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", Context.UserIdentifier, message);
        }
    }
}
