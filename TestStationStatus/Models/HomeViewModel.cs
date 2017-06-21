﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestStationStatus.Models
{
    public class HomeViewModel
    {
        public HttpPostedFileBase[] filesA;
        public HttpPostedFileBase[] filesB;

        public HomeViewModel()
        {

        }

        public HomeViewModel(HomeViewModel model)
        {
            filesA = model.filesA;
            filesB = model.filesB;
        }


    }
}