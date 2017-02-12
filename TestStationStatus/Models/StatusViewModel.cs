using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestStationStatus.Models
{
    public class StatusViewModel
    {
        public string Name { get; set; }
        public string ApplicationStatus { get; set; }
        public string LastUpdateTime { get; set; }
        public string TestScript { get; set; }
        public string LogFile { get; set; }
        public List<string> StatusFile;
        public List<string> ResultsFile;
        public List<string> QueueItems;
        public List<string> MonitorFiles;

        public StatusViewModel()
        {
            StatusFile = new List<string>();
            ResultsFile = new List<string>();
            QueueItems = new List<string>();
            MonitorFiles = new List<string>();
        }

        public StatusViewModel(StatusModel model)
        {
            this.Name = model.Name;
            this.ApplicationStatus = model.ApplicationStatus;
            LastUpdateTime = model.LastUpdateTime;
            TestScript = model.TestScript;
            LogFile = model.LogFile;
            StatusFile = model.StatusFile;
            ResultsFile = model.ResultsFile;
            QueueItems = model.QueueItems;
            MonitorFiles = model.MonitorFiles;
        }

        public string LastUpdateTimeString
        {
            get
            {
                if (string.IsNullOrWhiteSpace (LastUpdateTime ))
                    return "";
                return TimeSpan.FromSeconds(double.Parse(LastUpdateTime)).ToString();
            }
        }


    }
}