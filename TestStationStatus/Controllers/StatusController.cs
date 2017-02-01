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
        // GET: Status
        public ActionResult Index()
        {
            if (model == null)
            {
                model = new StatusModel();

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
            }
            catch (Exception ex)
            {
                model.ApplicationStatus = "File locked";
            }


            return View(model);
        }
    }
}
