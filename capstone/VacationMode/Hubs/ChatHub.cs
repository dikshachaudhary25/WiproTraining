using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace VacationMode.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string senderId, string receiverId, object messageData)
        {
            
            
            
            
            await Clients.User(receiverId)
                .SendAsync("ReceiveMessage", senderId, messageData);

            await Clients.User(senderId)
                .SendAsync("ReceiveMessage", senderId, messageData);
        }
    }
}
