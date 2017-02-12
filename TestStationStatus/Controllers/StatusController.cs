﻿using Microsoft.AspNet.SignalR;
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
        LocalTestDataService localDataService;

        private static StatusViewModel _model;
        private System.Threading.Thread background;
        private yourAppHub hub = new yourAppHub();

        public StatusController()
        {
            localDataService = PoorMansIOC.GetLocalTestDataService(); // TODO: replace with IOC contanier
        }


        private void thread()
        {

            while (true)
            {
                var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                context.Clients.All.newMessage("test3");

                var context2 = GlobalHost.ConnectionManager.GetHubContext<yourAppHub>();
                IHubConnectionContext<dynamic> Clients = context2.Clients;
                Clients.All.refreshPage();

                System.Threading.Thread.Sleep(10000);

                


               /* var hubConnection = new HubConnection("http://localhost/SimpleChat");

                chat = hubConnection.CreateHubProxy("chat");

                chat.On<string>("newMessage",
                                msg => messages.Invoke(new Action(() =>
                                                                  messages.Items.Add(msg))));*/
            }
        }

        public ActionResult Cancel()
        {
            localDataService.KillCurrentTestCase();
            return Redirect(Request.UrlReferrer.ToString());
        }

        // GET: Status
        public ActionResult Index()
        {
            if (_model == null)
            {
                _model = new StatusViewModel();
                background = new System.Threading.Thread(new System.Threading.ThreadStart(thread));
                background.Start();
            }

            var model = localDataService.GetModelFromLocalFiles();
            _model = new StatusViewModel(model);

            return View(_model);
        }
    }
}