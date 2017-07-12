using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using TestStationStatusInfrastructure.HubPayload;

namespace TestStationStatusInfrastructure.Hubs
{
    /// <summary>
    /// Uses SignalR to refresh frequently changing items on the web page
    /// </summary>
    public class MonitorHub : Hub, IMonitorHub
    {
        public void refreshPage()
        {
            Clients.All.refreshPage();
        }

        public void HomeUpdated(HomeScreenUpdate newStatus)
        {
            Clients.All.statusAUpdated(newStatus);
        }

        public void statusBUpdated(string status)
        {
            Clients.All.statusBUpdated(status);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }


        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

    }
}