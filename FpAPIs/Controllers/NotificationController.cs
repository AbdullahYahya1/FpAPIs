using Business.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace FpAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        //private readonly IHubContext<BroadcastHub> _broadcastHubContext;
        //private readonly IHubContext<AdminHub> _AdminHub;

        //private readonly IHubContext<UserHub> _userHubContext;
        //private readonly IHttpContextAccessor _httpContextAccessor;

        //In the constructor => IHubContext<UserHub> userHubContext, 
        //public NotificationController(IHubContext<BroadcastHub> broadcastHubContext,IHubContext<AdminHub> AdminhubContext, IHttpContextAccessor httpContextAccessor)
        //{
        //    _broadcastHubContext = broadcastHubContext;
        //    _AdminHub = AdminhubContext;
        //    //_userHubContext = userHubContext;
        //}
        //[HttpPost("broadcast")]
        //public async Task<IActionResult> BroadcastMessage([FromBody] string message)
        //{
        //    await _broadcastHubContext.Clients.All.SendAsync("ReceiveBroadcastMessage", message);
        //    return Ok(new { Message = "Broadcast sent" });
        //}
        //[HttpPost("notifyUser")]
        //public async Task<IActionResult> NotifyUser(string userId, [FromBody] string message)
        //{
        //    await _userHubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);
        //    return Ok(new { Message = $"Notification sent to user {userId}" });
        //}
    }
}
