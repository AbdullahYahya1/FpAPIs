using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Hubs
{
    public class BroadcastHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("ReceiveBroadcastMessage", "Welcome to the hub!");
        }
        public async Task BroadcastMessage(string message)
        {
            try
            {
                await Clients.All.SendAsync("ReceiveBroadcastMessage", message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending broadcast message: {ex.Message}");
            }
        }

    }
}
