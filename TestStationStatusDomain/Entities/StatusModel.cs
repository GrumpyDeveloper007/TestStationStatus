using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TestStationStatus.Models
{
    public class StatusModel
    {
        public string Name { get; set; }
        public string ApplicationStatus { get; set; }
        public string LastUpdateTime { get; set; }
        public string WebQueryTime { get; set; }
        public string TestScript { get; set; }
        [DisplayName("Overnight test plan active")]
        public string TestPlanActive { get; set; }
        public string LogFile { get; set; }
        public List<string> StatusFile;
        public List<string> ResultsFile;
        public List<string> QueueItems;
        public List<string> MonitorFiles;
        public double QueueDuration;
        public double MonitorDuration;
        public double TestScriptLastDuration;
        public bool QueueDurationKnown;
        public bool MonitorDurationKnown;

        public StatusModel()
        {
            StatusFile = new List<string>();
            ResultsFile = new List<string>();
            QueueItems = new List<string>();
            MonitorFiles = new List<string>();
            ApplicationStatus = "";
            QueueDurationKnown = true;
            MonitorDurationKnown = true;
        }

        public string GetStatusMessage ()
        {
            return ApplicationStatus + ", queue : " + (MonitorFiles.Count() + QueueItems.Count()) + " Free to run a new test in : " + TimeUntilStationIsFreeString;
        }

        public string LastUpdateTimeString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(LastUpdateTime))
                    return ":" + WebQueryTime;
                return TimeSpan.FromSeconds(double.Parse(LastUpdateTime)).ToString() +" (" + TimeSpan.FromSeconds(TestScriptLastDuration).ToString() + ")" + " : " + WebQueryTime;
            }
        }

        public string TimeUntilStationIsFreeString
        {
            get
            {
                double timeLeftOnCurrentTest = 0;
                if (!string.IsNullOrWhiteSpace(LastUpdateTime) && TestScriptLastDuration > 0 && TestScriptLastDuration >= double.Parse(LastUpdateTime))
                {
                    timeLeftOnCurrentTest = TestScriptLastDuration - double.Parse(LastUpdateTime);
                }
                return TimeSpan.FromSeconds(QueueDuration + MonitorDuration + timeLeftOnCurrentTest).ToString(@"hh\:mm\:ss") + ((QueueDurationKnown && MonitorDurationKnown) ? "" : "?");
            }
        }

        public string QueueDurationString
        {
            get
            {
                return TimeSpan.FromSeconds(QueueDuration).ToString(@"hh\:mm\:ss") + (QueueDurationKnown ? "" : "?");
            }
        }

        public string MonitorDurationString
        {
            get
            {
                return TimeSpan.FromSeconds(MonitorDuration).ToString(@"hh\:mm\:ss") + (MonitorDurationKnown ? "" : "?");
            }
        }

    }
}