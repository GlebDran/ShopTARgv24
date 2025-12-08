using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;

namespace ShopTARgv24.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            var timestamp = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
            await Clients.All.SendAsync("ReceiveMessage", user, message, timestamp);
        }
    }
}