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

        public StatusModel()
        {
            StatusFile = new List<string>();
            ResultsFile = new List<string>();
            QueueItems = new List<string>();
            MonitorFiles = new List<string>();
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

        public string MonitorAndQueueDurationString
        {
            get
            {
                return TimeSpan.FromSeconds(QueueDuration+MonitorDuration).ToString();
            }
        }

        public string QueueDurationString
        {
            get
            {
                return TimeSpan.FromSeconds(QueueDuration).ToString();
            }
        }

        public string MonitorDurationString
        {
            get
            {
                return TimeSpan.FromSeconds(MonitorDuration).ToString();
            }
        }

    }
}