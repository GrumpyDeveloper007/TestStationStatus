using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestStationStatus.Models;
using TestStationStatusInfrastructure;
using TestStationStatusInfrastructure.Service;

namespace TestStationStatus.Controllers
{
    public class HomeController : Controller
    {
        private HomeViewModel _model;

        RefreshClientService _dataUpdatedClient;
        LocalTestDataService _LocalTestDataServiceA1;
        LocalTestDataService _LocalTestDataServiceB1;
        LocalTestDataService _LocalTestDataServiceA2;
        LocalTestDataService _LocalTestDataServiceB2;

        public HomeController(RefreshClientService refreshClientService, 
            LocalTestDataService localTestDataServiceA1,
        LocalTestDataService localTestDataServiceB1,
        LocalTestDataService localTestDataServiceA2,
        LocalTestDataService localTestDataServiceB2)
        {
            _dataUpdatedClient = refreshClientService;
            _LocalTestDataServiceA1 = localTestDataServiceA1;
            _LocalTestDataServiceB1 = localTestDataServiceB1;
            _LocalTestDataServiceA2 = localTestDataServiceA2;
            _LocalTestDataServiceB2 = localTestDataServiceB2;
        }

        public ActionResult Index()
        {
            _model = new HomeViewModel();
            if (_dataUpdatedClient.BackgroundWorker == null)
            {
                //new List<LocalTestDataService> { _LocalTestDataServiceA1, _LocalTestDataServiceB1, _LocalTestDataServiceA2, _LocalTestDataServiceB2 }
                _dataUpdatedClient.StartService();
            }

            while (_dataUpdatedClient.LatestModel==null )
            {
                System.Threading.Thread.Sleep(1000);
            }

            _model.Model = _dataUpdatedClient.LatestModel;

            return View(_model);
        }

        [HttpPost]
        public ActionResult Upload2(int id, HomeViewModel test)
        {
            string IP = Request.UserHostName;
            var file = Request.Files;
            string userName = null;

            if (User != null && User.Identity != null && !string.IsNullOrWhiteSpace(User.Identity.Name))
            {
                userName = User.Identity.Name;
            }

            // Verify that the user selected a file
            _dataUpdatedClient.UploadFile(id, file, IP, userName);
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult Cancel(int id, HomeViewModel test)
        {
            _dataUpdatedClient.KillCurrentTestCase(id);
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact information.";

            return View();
        }
    }
}