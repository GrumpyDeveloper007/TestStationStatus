using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStationStatusInfrastructure.Hubs;

namespace TestStationStatusInfrastructure.Service
{
    public class RefreshClientService
    {

        public System.Threading.Thread BackgroundWorker { get; set; }

        public bool Running { get; set; }

        private void thread()
        {

            while (Running == true)
            {
                var context2 = GlobalHost.ConnectionManager.GetHubContext<MonitorHub>();
                IHubConnectionContext<dynamic> Clients = context2.Clients;
                Clients.All.refreshPage();

                bool logged = false;
                while (logged == false)
                {
                    try
                    {
                        System.IO.File.AppendAllText(@"C:\kf2_ats\weblog.txt", DateTime.Now.ToString("hh:mm:ss.fff") + "refresh page\r\n");
                        logged = true;
                    }
                    catch (Exception ex)
                    { }
                }

                System.Threading.Thread.Sleep(10000);
            }
        }

        public void StartService()
        {
            if (Running == false)
            {
                //System.IO.File.AppendAllText(@"C:\kf2_ats\weblog.txt", DateTime.Now.ToString("hh:mm:ss.fff") + "new instance\r\n");
                BackgroundWorker = new System.Threading.Thread(new System.Threading.ThreadStart(thread));
                Running = true;
                BackgroundWorker.Start();
            }
        }
    }
}
