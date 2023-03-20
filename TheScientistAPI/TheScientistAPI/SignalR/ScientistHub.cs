using Microsoft.AspNetCore.SignalR;

namespace TheScientistAPI.SignalR
{
    public class ScientistHub:Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
