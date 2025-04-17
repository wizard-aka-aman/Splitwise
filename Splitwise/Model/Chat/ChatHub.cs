using Microsoft.AspNetCore.SignalR;

namespace Splitwise.Model.Chat
{
    public class ChatHub :Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("RecieveMessage" , $"{Context.ConnectionId} has Joined")
        }
    }
}
