using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestStationStatus.Models
{
    public class StatusModel
    {
        public string Name { get; set; }
        public string ApplicationStatus { get; set; }
        private List<string> _statusFile;


        public StatusModel()
        {
            _statusFile = new List<string>();
        }


        public List<string> StatusFile
        {
            get
            {
                return _statusFile;
            }
            set
            {
                _statusFile = value;
            }
        }

    }
}