using Microsoft.AspNetCore.SignalR;

namespace KMwareWeb01.Hubs
{
    public sealed class UpdateHub : Hub
    {
        public async Task SentMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ReceivedMessage", $"{Context.ConnectionId} joined");
        }
    }
}
