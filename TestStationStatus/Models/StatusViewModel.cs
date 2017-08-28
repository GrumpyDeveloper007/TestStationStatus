using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TestStationStatus.Models
{
    public class StatusViewModel
    {
        public string PCName { get; set; }
        public string Name { get; set; }
        public string ApplicationStatus { get; set; }
        public string LastUpdateTime { get; set; }
        public string WebQueryTime { get; set; }
        public string TestScript { get; set; }
        [DisplayName("Scheduled Test plan active")]
        public string TestPlanActive { get; set; }
        public string LogFile { get; set; }
        public List<string> StatusFile;
        public List<string> ResultsFile;
        public List<string> QueueItems;
        public List<string> MonitorFiles;
        public List<string> FailMessages;
        public HttpPostedFileBase file;
        public string LastUpdateTimeString { get; set; }
        public string ScriptStyle { get; set; }

        public StatusViewModel()
        {
            StatusFile = new List<string>();
            ResultsFile = new List<string>();
            QueueItems = new List<string>();
            MonitorFiles = new List<string>();
            FailMessages= new List<string>();
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
            ScriptStyle = model.ScriptStyle;
            FailMessages = model.FailMessages;
            TestPlanActive = model.TestPlanActive;
            WebQueryTime = model.WebQueryTime;
            LastUpdateTimeString = model.LastUpdateTimeString;
        }
    }
}