using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestStationStatus.Models
{
    public class HomeViewModel
    {
        public HttpPostedFileBase[] filesA;
        public HttpPostedFileBase[] filesB;
        public HttpPostedFileBase[] filesA2;
        public HttpPostedFileBase[] filesB2;

        public HomeViewModel()
        {

        }

        public HomeViewModel(HomeViewModel model)
        {
            filesA = model.filesA;
            filesB = model.filesB;
            filesA2 = model.filesA2;
            filesB2 = model.filesB2;
        }


    }
}