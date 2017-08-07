﻿using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TestStationStatus.Models;

namespace TestStationStatusInfrastructure.Service
{
    /// <summary>
    /// Communicates with another application using files. 
    /// This could be replaced with a rest API in the future, however for now I need to support the application
    /// while making the absolute minimum changes to the external application and the simplest way to achieve 
    /// this is to simply write to a few files.
    /// </summary>
    public class LocalTestDataService
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private IpAddressService _IpAddressService;

        public string WorkingFolder = @"C:\kf2_ats";
        public StatusModel CurrentModel = new StatusModel();
        public string PCName { get; set; }

        public LocalTestDataService(IpAddressService ipAddressService)
        {
            _IpAddressService = ipAddressService;
        }

        public void KillCurrentTestCase()
        {
            System.IO.File.WriteAllText(WorkingFolder + @"\e420\KillSwitch.txt", "Stop");
        }

        public void UploadFile(HttpPostedFileBase[] files, string iPAddress)
        {
            string computerName = _IpAddressService.DetermineComputerName(iPAddress);

            foreach (HttpPostedFileBase file in files)
            {
                // extract only the file name
                var fileName = Path.GetFileName(file.FileName);
                // store the file 
                var path = Path.Combine(WorkingFolder + @"\E420MonitorTestPlan", fileName);
                file.SaveAs(path);

                string[] lines = File.ReadAllLines(path);
                lines[0] = "{<TestMeta><ComputerName>" + computerName + "</ComputerName></TestMeta>}" + lines[0];

                File.WriteAllLines(path, lines);
            }
        }

        public void UploadFile(HttpFileCollectionBase files, string iPAddress)
        {
            string computerName = _IpAddressService.DetermineComputerName(iPAddress);

            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                // extract only the file name
                var fileName = Path.GetFileName(file.FileName);
                // store the file 
                var path = Path.Combine(WorkingFolder + @"\E420MonitorTestPlan", fileName);
                file.SaveAs(path);

                string[] lines = File.ReadAllLines(path);
                lines[0] = "{<TestMeta><ComputerName>" + computerName + "</ComputerName></TestMeta>}" + lines[0];

                File.WriteAllLines(path, lines);
            }
        }

        public bool IsMidnightRunning()
        {
            if (System.IO.File.Exists(WorkingFolder + @"\Controller\MidnightLock.lst"))
            {
                return true;
            }
            if (System.IO.File.Exists(WorkingFolder + @"\Controller-AUSYDPC419\MidnightLock.lst"))
            {
                return true;
            }
            return false;
        }

        public StatusModel GetModelFromLocalFiles()
        {
            return CurrentModel;
        }

        public StatusModel UpdateModel()
        {
            try

            {
                var model = new StatusModel();
                model.TestPlanActive = IsMidnightRunning().ToString();
                model.WebQueryTime = DateTime.Now.ToLongTimeString();
                model.MonitorDuration = CurrentModel.MonitorDuration;
                model.TestScriptLastDuration = CurrentModel.TestScriptLastDuration;
                model.QueueDuration = CurrentModel.QueueDuration;
                model.MonitorDurationKnown = CurrentModel.MonitorDurationKnown;

                if (System.IO.File.Exists(WorkingFolder + @"\e420\ApplicationSummary.txt"))
                {
                    var lines = System.IO.File.ReadAllLines(WorkingFolder + @"\e420\ApplicationSummary.txt");
                    model.StatusFile.AddRange(lines);
                }

                if (System.IO.File.Exists(WorkingFolder + @"\e420\ApplicationStatus.txt"))
                {
                    var status = System.IO.File.ReadAllLines(WorkingFolder + @"\e420\ApplicationStatus.txt");
                    model.ApplicationStatus = status[0];
                    if (status.Count() > 1)
                    {
                        if (!string.IsNullOrWhiteSpace(status[1]))
                            model.LastUpdateTime = status[1];
                    }
                    if (status.Count() > 2)
                    {
                        if (!string.IsNullOrWhiteSpace(status[2]))
                            model.TestScript = Path.GetFileName(status[2]);
                    }
                    if (status.Count() > 3)
                    {
                        if (!string.IsNullOrWhiteSpace(status[3]))
                            model.LogFile = status[3];
                    }
                }
                else
                {
                    model.ApplicationStatus = "No status to report, please run a test script";
                }

                if (System.IO.File.Exists(WorkingFolder + @"\e420\ApplicationResults.txt"))
                {
                    var lines = System.IO.File.ReadAllLines(WorkingFolder + @"\e420\ApplicationResults.txt");
                    model.ResultsFile.AddRange(lines);
                }

                var queueItems = System.IO.Directory.GetFiles(WorkingFolder + @"\queue\", "*.TST");
                var completedItems = System.IO.Directory.GetFiles(WorkingFolder + @"\queue\", "*.LST");
                model.QueueItems.Clear();
                foreach (string item in queueItems)
                {
                    bool found = false;
                    string fileName = System.IO.Path.GetFileName(item);
                    foreach (string completedItem in completedItems)
                    {
                        if (completedItem.Contains(fileName))
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found == false)
                    {

                        model.QueueItems.Add(Path.GetFileName(fileName));
                    }
                }

                var monitorFiles = System.IO.Directory.GetFiles(WorkingFolder + @"\E420MonitorTestPlan", "*.TST");
                model.MonitorFiles = new List<string>();
                foreach (var file in monitorFiles)
                {
                    model.MonitorFiles.Add(Path.GetFileName(file));
                }
                model.QueueDuration = 0;
                model.QueueDurationKnown = true;
                model.MonitorDuration = 0;
                model.MonitorDurationKnown = true;
                CurrentModel = model;
            }
            catch (Exception ex)
            {
                CurrentModel.ApplicationStatus = "File locked";
                _logger.Log(LogLevel.Error, ex);
            }


            return CurrentModel;
        }

    }
}