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
        LocalTestDataService _localDataServiceB;
        LocalTestDataService _localDataService;


        public System.Threading.Thread BackgroundWorker { get; set; }

        public bool Running { get; set; }

        private void thread()
        {

            while (Running == true)
            {
                _localDataService.UpdateModel();
                _localDataServiceB.UpdateModel();

                var context2 = GlobalHost.ConnectionManager.GetHubContext<MonitorHub, IMonitorHub>();
                var Clients = context2.Clients;
                Clients.All.refreshPage();

                bool logged = false;
                while (logged == false)
                {
                    try
                    {
                       // System.IO.File.AppendAllText(@"C:\kf2_ats\weblog.txt", DateTime.Now.ToString("hh:mm:ss.fff") + "refresh page\r\n");
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
                _localDataService = PoorMansIOC.GetLocalTestDataService(); // TODO: replace with IOC container

                _localDataServiceB = PoorMansIOC.GetLocalTestDataService2(); // TODO: replace with IOC container
                _localDataServiceB.WorkingFolder = @"C:\kf2_atsB";


                //System.IO.File.AppendAllText(@"C:\kf2_ats\weblog.txt", DateTime.Now.ToString("hh:mm:ss.fff") + "new instance\r\n");
                BackgroundWorker = new System.Threading.Thread(new System.Threading.ThreadStart(thread));
                Running = true;
                BackgroundWorker.Start();
            }
        }
    }
}
