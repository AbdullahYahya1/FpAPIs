using Business.Hubs;
using Business.IServices;
using Common;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class NotificationService:INotificationService
    {
        private readonly IHubContext<AdminHub> _adminHub;
        public NotificationService(IHubContext<AdminHub> adminHub)
        {
            _adminHub = adminHub;
        }
        //[CustomAuthorize([UserType.Manager])]
        public async Task NotifyAdminAsync(string message)
        {
            await _adminHub.Clients.All.SendAsync("ReceiveBroadcastMessage", message);
        }
    }
}
