﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace TestStationStatus
{
    public class yourAppHub : Hub
    {

        public void refreshPage()
        {
            Clients.All.refreshPage();
        }

    }

}