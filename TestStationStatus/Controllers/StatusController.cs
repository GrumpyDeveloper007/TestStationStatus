using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TestStationStatus.Models;
using TestStationStatusInfrastructure;
using TestStationStatusInfrastructure.Service;

namespace TestStationStatus.Controllers
{
    public class StatusController : Controller
    {
        LocalTestDataService _localDataService;
        RefreshClientService _dataUpdatedClient;

        private static StatusViewModel _model;

        public StatusController()
        {
            _localDataService = PoorMansIOC.GetLocalTestDataService(0); // TODO: replace with IOC container
            _dataUpdatedClient = PoorMansIOC.GetRefreshDataService();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //System.IO.File.AppendAllText(@"C:\kf2_ats\weblog.txt", DateTime.Now.ToString("hh:mm:ss.fff") + "controller destroyed\r\n");
                //_dataUpdatedClient.Running = false;
            }

            base.Dispose(disposing);
        }

        public ActionResult Cancel()
        {
            _localDataService.KillCurrentTestCase();
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        public ActionResult Upload(StatusViewModel model, HttpPostedFileBase[] file)
        {
            string IP = Request.UserHostName;
            // Verify that the user selected a file
            if (file != null && file[0]!=null && file[0].ContentLength > 0)
            {
                _localDataService.UploadFile(file,IP);
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        public ActionResult Upload2()
        {
            string IP = Request.UserHostName;
            var file = Request.Files;
            // Verify that the user selected a file
            if (file != null && file[0] != null && file[0].ContentLength > 0)
            {
                _localDataService.UploadFile(file, IP);
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        // GET: Status
        public ActionResult Index()
        {
            if (_model == null)
            {
                _model = new StatusViewModel();
            }

            if (_dataUpdatedClient.BackgroundWorker == null)
            {
                _dataUpdatedClient.StartService();
            }


            var model = _localDataService.GetModelFromLocalFiles();
            _model = new StatusViewModel(model);

            return View(_model);
        }

        public ActionResult Status()
        {
            if (_model == null)
            {
                _model = new StatusViewModel();
            }

            if (_dataUpdatedClient.BackgroundWorker == null)
            {
                _dataUpdatedClient.StartService();
            }


            var model = _localDataService.GetModelFromLocalFiles();
            _model = new StatusViewModel(model);

            return View(_model);
        }

    }
}