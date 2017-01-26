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
            var lines = System.IO.File.ReadAllLines(@"C:\kf2_ats\queue\ApplicationSummary.txt");
            model.ApplicationStatus = "Idle";
            model.StatusFile.AddRange(lines);
            if (System.IO.File.Exists(@"C:\kf2_ats\queue\ApplicationStatus.txt"))
            {
                var status = System.IO.File.ReadAllLines(@"C:\kf2_ats\queue\ApplicationStatus.txt");
            }


            return View(model);
        }
    }
}
