using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStationStatus.Models;

namespace TestStationStatusInfrastructure.Service
{
    public class LocalTestDataService
    {
        public void KillCurrentTestCase()
        {
            System.IO.File.WriteAllText(@"C:\kf2_ats\e420\KillSwitch.txt", "Stop");
        }

        public StatusModel GetModelFromLocalFiles()
        {
            StatusModel model = new StatusModel();
            try

            {
                model.StatusFile.Clear();
                model.ResultsFile.Clear();
                model.ApplicationStatus = "";

                var lines = System.IO.File.ReadAllLines(@"C:\kf2_ats\e420\ApplicationSummary.txt");
                model.StatusFile.AddRange(lines);
                if (System.IO.File.Exists(@"C:\kf2_ats\e420\ApplicationStatus.txt"))
                {
                    var status = System.IO.File.ReadAllLines(@"C:\kf2_ats\e420\ApplicationStatus.txt");
                    model.ApplicationStatus = status[0];
                    if (status.Count() > 1)
                    {
                        model.LastUpdateTime = status[1];
                    }
                    if (status.Count() > 2)
                    {
                        model.TestScript = status[2];
                    }
                    if (status.Count() > 3)
                    {
                        model.LogFile = status[3];
                    }
                }

                if (System.IO.File.Exists(@"C:\kf2_ats\e420\ApplicationResults.txt"))
                {
                    lines = System.IO.File.ReadAllLines(@"C:\kf2_ats\e420\ApplicationResults.txt");
                    model.ResultsFile.AddRange(lines);
                }

                var queueItems = System.IO.Directory.GetFiles(@"C:\kf2_ats\queue\", "*.TST");
                var completedItems = System.IO.Directory.GetFiles(@"C:\kf2_ats\queue\", "*.LST");
                model.QueueItems.Clear();
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
                        model.QueueItems.Add(fileName);
                    }
                }

                var monitorFiles = System.IO.Directory.GetFiles(@"C:\KF2_ATS\E420MonitorTestPlan", "*.TST");
                model.MonitorFiles = monitorFiles.ToList();

            }
            catch (Exception ex)
            {
                model.ApplicationStatus = "File locked";
            }
            return model;
        }

    }
}
