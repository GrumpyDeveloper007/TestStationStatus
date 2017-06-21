using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestStationStatusInfrastructure;
using TestStationStatusInfrastructure.Service;

namespace TestStationStatus.Controllers
{
    public class HomeController : Controller
    {

        RefreshClientService _dataUpdatedClient;

        public HomeController()
        {
            _dataUpdatedClient = PoorMansIOC.GetRefreshDataService();
        }

        public ActionResult Index()
        {
            if (_dataUpdatedClient.BackgroundWorker == null)
            {
                _dataUpdatedClient.StartService();
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}