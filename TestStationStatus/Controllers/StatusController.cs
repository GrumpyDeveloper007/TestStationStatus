using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
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
    public class StatusController : Controller
    {
        LocalTestDataService _localDataService;
        RefreshClientService _dataUpdatedClient;

        private static StatusViewModel _model;

        public StatusController()
        {
            _localDataService = PoorMansIOC.GetLocalTestDataService(); // TODO: replace with IOC container
            _dataUpdatedClient = PoorMansIOC.GetRefreshDataService();
        }

        public ActionResult Cancel()
        {
            _localDataService.KillCurrentTestCase();
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
    }
}