using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;

namespace TestStationStatusInfrastructure.Hubs
{
    public class MonitorHub : Hub, IMonitorHub
    {
        public void refreshPage()
        {
            Clients.All.refreshPage();
        }

        public void statusAUpdated(string status)
        {
            Clients.All.statusAUpdated(status);
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