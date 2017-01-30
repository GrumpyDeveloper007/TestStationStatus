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
        public string LastUpdateTime { get; set; }
        public string TestScript { get; set; }
        public string LogFile { get; set; }
        private List<string> _statusFile;
        private List<string> _ResultsFile;


        public StatusModel()
        {
            _statusFile = new List<string>();
            _ResultsFile = new List<string>();
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

        public List<string> ResultsFile
        {
            get
            {
                return _ResultsFile;
            }
            set
            {
                _ResultsFile = value;
            }
        }

    }
}