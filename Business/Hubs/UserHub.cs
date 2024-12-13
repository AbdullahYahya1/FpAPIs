using Microsoft.AspNetCore.SignalR;

public class UserHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        Console.WriteLine($"User connected with UserIdentifier: {userId}");
        await base.OnConnectedAsync();
    }
    public async Task SendNotificationToUser(string message)
    {
        //var userId = Context.UserIdentifier;
        //if (userId != null)
        //{
        //    await Clients.User(userId).SendAsync("ReceiveNotification", message);
        //}
        await Clients.All.SendAsync("ReceiveNotification", message);
    }
}
