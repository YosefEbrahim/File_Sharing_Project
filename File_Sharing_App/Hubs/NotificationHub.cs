using Microsoft.AspNetCore.SignalR;

namespace File_Sharing_App.Hubs
{
    public class NotificationHub:Hub
    {
        public override Task OnConnectedAsync()
        {
            Context.Items.Add(Context.UserIdentifier, Context.ConnectionId);
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Context.Items.Remove(Context.UserIdentifier);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
