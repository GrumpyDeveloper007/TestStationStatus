﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace TestStationStatus
{
    [HubName("monitor")]
    public class MonitorHub : Hub
    {

    }
}