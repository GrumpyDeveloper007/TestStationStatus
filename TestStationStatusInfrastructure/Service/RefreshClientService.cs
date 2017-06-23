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
        const int _RefreshDataIntervalMs = 10000;

        LocalTestDataService _localDataServiceB;
        LocalTestDataService _localDataService;
        ServerDataService _ServerDataService;
        IHubConnectionContext<IMonitorHub> _MonitorHub;

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

        private double CalculateDuration(List<string> fileNames, ref bool known)
        {
            double duration = 0;
            known = true;

            foreach (var fileName in fileNames)
            {
                double fileDuration = _ServerDataService.GetDurationOfTestCase(fileName);
                if (fileDuration == 0)
                {
                    known = false;
                    _logger.Log(LogLevel.Debug, "unknown duration:" + fileName + "duration:" + duration);
                }
                duration += fileDuration;

            }
            return duration;
        }

        private bool IsListDifferent(List<string> newList, List<string> oldList)
        {
            if (newList.Count != oldList.Count)
                return true;
            for (int i = 0; i < newList.Count; i++)
            {
                if (newList[i] != oldList[i])
                {
                    return true;
                }
            }
            return false;
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
                    modelA.MonitorDuration = _modelA.MonitorDuration;
                    modelB.MonitorDuration = _modelB.MonitorDuration;
                    modelA.TestScriptLastDuration = _modelA.TestScriptLastDuration;
                    modelB.TestScriptLastDuration = _modelB.TestScriptLastDuration;
                    modelA.QueueDuration = _modelA.QueueDuration;
                    modelB.QueueDuration = _modelB.QueueDuration;
                    modelA.MonitorDurationKnown = _modelA.MonitorDurationKnown;
                    modelB.MonitorDurationKnown = _modelB.MonitorDurationKnown;

                    if (IsListDifferent(modelA.MonitorFiles, _modelA.MonitorFiles))
                        modelA.MonitorDuration = CalculateDuration(modelA.MonitorFiles, ref modelA.MonitorDurationKnown);
                    if (IsListDifferent(modelB.MonitorFiles, _modelB.MonitorFiles))
                        modelB.MonitorDuration = CalculateDuration(modelB.MonitorFiles, ref modelB.MonitorDurationKnown);
                    if (IsListDifferent(modelA.QueueItems, _modelA.QueueItems))
                        modelA.QueueDuration = CalculateDuration(modelA.QueueItems, ref modelA.QueueDurationKnown);
                    if (IsListDifferent(modelB.QueueItems, _modelB.QueueItems))
                        modelB.QueueDuration = CalculateDuration(modelB.QueueItems, ref modelB.QueueDurationKnown);
                    if (!string.IsNullOrWhiteSpace(modelA.TestScript))
                        modelA.TestScriptLastDuration = _ServerDataService.GetDurationOfTestCase(modelA.TestScript);
                    if (!string.IsNullOrWhiteSpace(modelB.TestScript))
                        modelB.TestScriptLastDuration = _ServerDataService.GetDurationOfTestCase(modelB.TestScript);

                    _MonitorHub.All.refreshPage();


                    //if (modelA.ApplicationStatus != _modelA.ApplicationStatus)
                    {
                        _MonitorHub.All.statusAUpdated(modelA.ApplicationStatus + ", queue : " + (modelA.MonitorFiles.Count() + modelA.QueueItems.Count()) + " Free to run a new test in : " + modelA.TimeUntilStationIsFreeString);
                    }

                    //if (modelB.ApplicationStatus != _modelB.ApplicationStatus)
                    {
                        _MonitorHub.All.statusBUpdated(modelB.ApplicationStatus + ", queue : " + (modelB.MonitorFiles.Count() + modelB.QueueItems.Count()) + " Free to run a new test in : " + modelB.TimeUntilStationIsFreeString);
                    }
                    _modelA = modelA;
                    _modelB = modelB;

                    System.Threading.Thread.Sleep(_RefreshDataIntervalMs);
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, ex);
                }
            }
        }

        public void StartService()
        {
            if (Running == false)
            {
                _logger.Log(LogLevel.Info, "Background service started");

                var context = GlobalHost.ConnectionManager.GetHubContext<MonitorHub, IMonitorHub>();
                _MonitorHub = context.Clients;

                _ServerDataService = PoorMansIOC.GetServerDataService();

                var ip = PoorMansIOC.GetIpAddressService();
                var pcs = _ServerDataService.GetReverseDNSFailsAsList();
                ip.UpdateLoopUpsToTry(pcs.ToArray());

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