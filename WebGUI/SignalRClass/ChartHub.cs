using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebGUI.SignalRClass
{
    public class ChartHub : Hub
    {
        public async Task asdasd(string json)
        {
           await Clients.All.SendAsync("RestApiMsg", json);
        }
    }
}
