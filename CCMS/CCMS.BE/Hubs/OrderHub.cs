using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CCMS.BE.Hubs
{
    public class OrderHub : Hub
    {
        public async Task SendOrderUpdate()
        {
            await Clients.All.SendAsync("ReceiveOrderUpdate");
        }
    }
}
