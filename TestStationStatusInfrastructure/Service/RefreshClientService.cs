using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStationStatus.Models;
using TestStationStatusDomain.Entities;
using TestStationStatusInfrastructure.Hubs;

namespace TestStationStatusInfrastructure.Service
{
    /// <summary>
    /// A background task that monitors the external application and local state of the test station
    /// </summary>
    public class RefreshClientService
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        LocalTestDataService _localDataServiceB;
        LocalTestDataService _localDataService;
        ServerDataService _ServerDataService;

        StatusModel _modelA = new StatusModel();
        StatusModel _modelB = new StatusModel();


        public System.Threading.Thread BackgroundWorker { get; set; }

        public bool Running { get; set; }

        private void SaveDuration(StatusModel model)
        {
            string errorMessage;
            if (model.TestScript != null && model.LastUpdateTime != null)
            {
                TestDuration completedTest = new TestDuration
                { DurationSeconds = double.Parse(model.LastUpdateTime), TestCaseName = model.TestScript, TestStationName = Environment.MachineName };
                errorMessage = _ServerDataService.SaveDuration(completedTest);
            }
        }

        private double CalculateDuration(List<string> fileNames)
        {
            double duration = 0;

            foreach (var fileName in fileNames)
            {
                duration += _ServerDataService.GetDurationOfTestCase(fileName);
                _logger.Log(LogLevel.Debug, "cal duration:" + fileName + "duration:" + duration);
            }
            return duration;
        }

        private void thread()
        {

            while (Running == true)
            {
                try
                {
                    var modelA = _localDataService.UpdateModel();
                    var modelB = _localDataServiceB.UpdateModel();

                    if (modelA.ApplicationStatus.Contains("Complete") || modelA.ApplicationStatus.Contains("Reading from EmpWin"))
                    {
                        SaveDuration(modelA);
                    }

                    if (modelB.ApplicationStatus.Contains("Complete") || modelB.ApplicationStatus.Contains("Reading from EmpWin"))
                    {
                        SaveDuration(modelB);
                    }
                    modelA.MonitorDuration = CalculateDuration(modelA.MonitorFiles);
                    modelB.MonitorDuration = CalculateDuration(modelB.MonitorFiles);
                    modelA.QueueDuration = CalculateDuration(modelA.QueueItems);
                    modelB.QueueDuration = CalculateDuration(modelB.QueueItems);
                    modelA.TestScriptLastDuration = _ServerDataService.GetDurationOfTestCase(modelA.TestScript);
                    modelB.TestScriptLastDuration = _ServerDataService.GetDurationOfTestCase(modelB.TestScript);

                    var context2 = GlobalHost.ConnectionManager.GetHubContext<MonitorHub, IMonitorHub>();
                    var Clients = context2.Clients;
                    Clients.All.refreshPage();


                    //if (modelA.ApplicationStatus != _modelA.ApplicationStatus)
                    {
                        Clients.All.statusAUpdated(modelA.ApplicationStatus + ", queue : " + (modelA.MonitorFiles.Count() + modelA.QueueItems.Count()) + " duration : " + modelA.MonitorAndQueueDurationString);
                    }

                    //if (modelB.ApplicationStatus != _modelB.ApplicationStatus)
                    {
                        Clients.All.statusBUpdated(modelB.ApplicationStatus + ", queue : " + (modelB.MonitorFiles.Count() + modelB.QueueItems.Count()) + " duration : " + modelB.MonitorAndQueueDurationString);
                    }
                    _modelA = modelA;
                    _modelB = modelB;



                    // System.IO.File.AppendAllText(@"C:\kf2_ats\weblog.txt", DateTime.Now.ToString("hh:mm:ss.fff") + "refresh page\r\n");


                    System.Threading.Thread.Sleep(10000);
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error , ex);
                }
            }
        }

        public void StartService()
        {
            if (Running == false)
            {
                _logger.Log(LogLevel.Info, "Background service started");
                _ServerDataService = PoorMansIOC.GetServerDataService();
                _localDataService = PoorMansIOC.GetLocalTestDataService(); // TODO: replace with IOC container

                _localDataServiceB = PoorMansIOC.GetLocalTestDataService2(); // TODO: replace with IOC container
                _localDataServiceB.WorkingFolder = @"C:\kf2_atsB";

                BackgroundWorker = new System.Threading.Thread(new System.Threading.ThreadStart(thread));
                Running = true;
                BackgroundWorker.Start();
            }
        }
    }
}