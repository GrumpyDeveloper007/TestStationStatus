using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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

        List<LocalTestDataService> _localDataService= new List<LocalTestDataService>();
        ServerDataService _ServerDataService;
        IpAddressService _IpAddressService;
        IHubConnectionContext<IMonitorHub> _MonitorHub;


        public RefreshClientService(ServerDataService serverDataService, IpAddressService ipAddressService)
        {
            _ServerDataService = serverDataService;
            _IpAddressService = ipAddressService;
        }

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


        private void RefreshModel(LocalTestDataService service)
        {
            var oldModel = service.CurrentModel;
            var model = service.UpdateModel();

            if (model.ApplicationStatus.Contains("Complete") || model.ApplicationStatus.Contains("Reading from EmpWin"))
            {
                SaveDuration(model);
            }

            //if (IsListDifferent(modelA.MonitorFiles, _modelA.MonitorFiles))
            model.MonitorDuration = CalculateDuration(model.MonitorFiles, ref model.MonitorDurationKnown);
            //if (IsListDifferent(modelA.QueueItems, _modelA.QueueItems))
            model.QueueDuration = CalculateDuration(model.QueueItems, ref model.QueueDurationKnown);
            //if (!string.IsNullOrWhiteSpace(modelA.TestScript))
            model.TestScriptLastDuration = _ServerDataService.GetDurationOfTestCase(model.TestScript);


        }

        private void thread()
        {

            while (Running == true)
            {
                try
                {
                    foreach (var item in _localDataService)
                    {
                        RefreshModel(item);
                    }

                    _MonitorHub.All.refreshPage();

                    try
                    {


                        _MonitorHub.All.Updated(_localDataService[0].CurrentModel.ApplicationStatus + ", queue : " + (_localDataService[0].CurrentModel.MonitorFiles.Count() + _localDataService[0].CurrentModel.QueueItems.Count()) + " Free to run a new test in : " + _localDataService[0].CurrentModel.TimeUntilStationIsFreeString, _localDataService[0].CurrentModel.TestScript,
                            _localDataService[1].CurrentModel.ApplicationStatus + ", queue : " + (_localDataService[1].CurrentModel.MonitorFiles.Count() + _localDataService[1].CurrentModel.QueueItems.Count()) + " Free to run a new test in : " + _localDataService[1].CurrentModel.TimeUntilStationIsFreeString, _localDataService[1].CurrentModel.TestScript,
                            _localDataService[2].CurrentModel.ApplicationStatus + ", queue : " + (_localDataService[2].CurrentModel.MonitorFiles.Count() + _localDataService[2].CurrentModel.QueueItems.Count()) + " Free to run a new test in : " + _localDataService[2].CurrentModel.TimeUntilStationIsFreeString, _localDataService[2].CurrentModel.TestScript,
                            _localDataService[3].CurrentModel.ApplicationStatus + ", queue : " + (_localDataService[3].CurrentModel.MonitorFiles.Count() + _localDataService[3].CurrentModel.QueueItems.Count()) + " Free to run a new test in : " + _localDataService[3].CurrentModel.TimeUntilStationIsFreeString, _localDataService[3].CurrentModel.TestScript
                            );
                    }
                    catch (Exception ex)
                    {
                        _logger.Log(LogLevel.Error, ex, "Error in signalR");
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, ex);
                }
                System.Threading.Thread.Sleep(_RefreshDataIntervalMs);
            }
        }

        public void KillCurrentTestCase(int index)
        {
            _localDataService[index].KillCurrentTestCase();
        }

        public void UploadFile(int index, HttpFileCollectionBase files, string IP)
        {
            // Verify that the user selected a file
            if (files != null && files.Count>0 && files[0] != null && files[0].ContentLength > 0)
            {
                _localDataService[index].UploadFile(files, IP);
            }
        }

        public void StartService()
        {
            if (Running == false)
            {
                _logger.Log(LogLevel.Info, "Background service started");

                var context = GlobalHost.ConnectionManager.GetHubContext<MonitorHub, IMonitorHub>();
                _MonitorHub = context.Clients;

                var pcs = _ServerDataService.GetReverseDNSFailsAsList();
                _IpAddressService.UpdateLoopUpsToTry(pcs.ToArray());

                var localDataServiceA = GetLocalTestDataService(0); 
                localDataServiceA.WorkingFolder = @"\\ausydpc418\KF2_ATS";

                var localDataServiceB = GetLocalTestDataService(1); 
                localDataServiceB.WorkingFolder = @"\\ausydpc418\KF2_ATSB";

                var localDataServiceA2 = GetLocalTestDataService(2);
                localDataServiceA2.WorkingFolder = @"\\ausydpc419\KF2_ATS";

                var localDataServiceB2 = GetLocalTestDataService(3);
                localDataServiceB2.WorkingFolder = @"\\ausydpc419\KF2_ATSB";

                BackgroundWorker = new System.Threading.Thread(new System.Threading.ThreadStart(thread));
                Running = true;
                BackgroundWorker.Start();
            }
        }

        public LocalTestDataService GetLocalTestDataService(int index)
        {
            while (_localDataService.Count <= index)
            {
                _localDataService.Add(new LocalTestDataService(_IpAddressService));
            }
            return _localDataService[index];
        }
    }
}