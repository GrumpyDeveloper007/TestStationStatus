using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestStationStatus
{
    public class YourAppHub : Hub
    {
        public static void Refresh()
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext();
            hubContext.Clients.All.refreshPage();
        }
    }
}