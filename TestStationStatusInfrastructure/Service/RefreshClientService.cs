using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStationStatus.Models;
using TestStationStatusInfrastructure.Hubs;

namespace TestStationStatusInfrastructure.Service
{
    public class RefreshClientService
    {
        LocalTestDataService _localDataServiceB;
        LocalTestDataService _localDataService;

        StatusModel _modelA = new StatusModel();
        StatusModel _modelB = new StatusModel();


        public System.Threading.Thread BackgroundWorker { get; set; }

        public bool Running { get; set; }

        private void thread()
        {

            while (Running == true)
            {
                var modelA = _localDataService.UpdateModel();
                var modelB = _localDataServiceB.UpdateModel();


                var context2 = GlobalHost.ConnectionManager.GetHubContext<MonitorHub, IMonitorHub>();
                var Clients = context2.Clients;
                Clients.All.refreshPage();


                //if (modelA.ApplicationStatus != _modelA.ApplicationStatus)
                {
                    Clients.All.statusAUpdated(modelA.ApplicationStatus + ", queue : " + (modelA.MonitorFiles.Count() + modelA.QueueItems.Count()));
                }

                //if (modelB.ApplicationStatus != _modelB.ApplicationStatus)
                {
                    Clients.All.statusBUpdated(modelB.ApplicationStatus + ", queue : " + (modelB.MonitorFiles.Count() + modelB.QueueItems.Count()));
                }
                _modelA = modelA;
                _modelB = modelB;


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