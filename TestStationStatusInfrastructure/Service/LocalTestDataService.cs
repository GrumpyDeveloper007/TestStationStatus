﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TestStationStatus.Models;

namespace TestStationStatusInfrastructure.Service
{
    public class LocalTestDataService
    {
        public string WorkingFolder = @"C:\kf2_ats";
        StatusModel _CurrentModel = new StatusModel();

        public void KillCurrentTestCase()
        {
            System.IO.File.WriteAllText(WorkingFolder + @"\e420\KillSwitch.txt", "Stop");
        }

        public void UploadFile(HttpPostedFileBase[] files, string iPAddress)
        {
            string computerName = IpAddressService.DetermineComputerName(iPAddress);

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
            string computerName = IpAddressService.DetermineComputerName(iPAddress);

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
            return _CurrentModel;
        }

        public StatusModel UpdateModel()
        {
            try

            {
                _CurrentModel.StatusFile.Clear();
                _CurrentModel.ResultsFile.Clear();
                _CurrentModel.ApplicationStatus = "";
                _CurrentModel.TestPlanActive = IsMidnightRunning().ToString();
                _CurrentModel.WebQueryTime = DateTime.Now.ToLongTimeString();

                if (System.IO.File.Exists(WorkingFolder + @"\e420\ApplicationSummary.txt"))
                {
                    var lines = System.IO.File.ReadAllLines(WorkingFolder + @"\e420\ApplicationSummary.txt");
                    _CurrentModel.StatusFile.AddRange(lines);
                }

                if (System.IO.File.Exists(WorkingFolder + @"\e420\ApplicationStatus.txt"))
                {
                    var status = System.IO.File.ReadAllLines(WorkingFolder + @"\e420\ApplicationStatus.txt");
                    _CurrentModel.ApplicationStatus = status[0];
                    if (status.Count() > 1)
                    {
                        _CurrentModel.LastUpdateTime = status[1];
                    }
                    if (status.Count() > 2)
                    {
                        _CurrentModel.TestScript = status[2];
                    }
                    if (status.Count() > 3)
                    {
                        _CurrentModel.LogFile = status[3];
                    }
                }
                else
                {
                    _CurrentModel.ApplicationStatus = "No status to report, please run a test script";
                }

                if (System.IO.File.Exists(WorkingFolder + @"\e420\ApplicationResults.txt"))
                {
                    var lines = System.IO.File.ReadAllLines(WorkingFolder + @"\e420\ApplicationResults.txt");
                    _CurrentModel.ResultsFile.AddRange(lines);
                }

                var queueItems = System.IO.Directory.GetFiles(WorkingFolder + @"\queue\", "*.TST");
                var completedItems = System.IO.Directory.GetFiles(WorkingFolder + @"\queue\", "*.LST");
                _CurrentModel.QueueItems.Clear();
                foreach (string item in queueItems)
                {
                    bool found = false;
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(item);
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
                        _CurrentModel.QueueItems.Add(fileName);
                    }
                }

                var monitorFiles = System.IO.Directory.GetFiles(WorkingFolder + @"\E420MonitorTestPlan", "*.TST");
                _CurrentModel.MonitorFiles = monitorFiles.ToList();

            }
            catch (Exception ex)
            {
                _CurrentModel.ApplicationStatus = "File locked";
            }
            return _CurrentModel;
        }

    }
}