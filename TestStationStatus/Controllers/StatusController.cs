using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestStationStatus.Models;

namespace TestStationStatus.Controllers
{
    public class StatusController : Controller
    {

        private static StatusModel model;
        private System.Threading.Thread background;
        private yourAppHub hub = new yourAppHub();

        private void thread()
        {

            while (true)
            {
                System.Threading.Thread.Sleep(10);
                var context = GlobalHost.ConnectionManager.GetHubContext<yourAppHub>();
                context.Clients.All.refreshPage();
            }
        }

        public ActionResult Cancel()
        {
            System.IO.File.WriteAllText(@"C:\kf2_ats\e420\KillSwitch.txt","Stop");
            return Redirect(Request.UrlReferrer.ToString());
        }

        // GET: Status
        public ActionResult Index()
        {
            if (model == null)
            {
                model = new StatusModel();
                //background = new System.Threading.Thread(new System.Threading.ThreadStart(thread));
                //background.Start();
            }
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


            return View(model);
        }
    }
}