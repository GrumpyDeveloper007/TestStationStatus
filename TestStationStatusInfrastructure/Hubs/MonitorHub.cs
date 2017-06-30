using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;

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

        public void Updated(string statusA, string currentScriptA, string statusB, string currentScriptB,
            string statusA2, string currentScriptA2, string statusB2, string currentScriptB2)
        {
            Clients.All.statusAUpdated(statusA, currentScriptA, statusB, currentScriptB, statusA2, currentScriptA2, statusB2, currentScriptB2);
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