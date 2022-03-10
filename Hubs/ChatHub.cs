using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using RealTimeChat.ViewModels;

namespace RealTimeChat.Hubs
{
    public class ChatHub:Hub
    {
        public async Task SendMessage(MessageViewModel model)
        {
            IEnumerable<string> users = new List<string>() {model.SenderId.ToString(),model.ReceiverId.ToString()};

            await Clients.Users(users).SendAsync("ReceiveMessage",model);
        }
    }
}
