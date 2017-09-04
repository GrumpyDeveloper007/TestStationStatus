using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStationStatusInfrastructure.HubPayload
{
    public class HomeScreenStationStatus
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string CurrentScript { get; set; }
        public string ScriptStyle { get; set; }
        public int Id { get; set; }

        public string NameId { get; set; }
        public string StatusId { get; set; }
        public string CurrentScriptId { get; set; }


        public HomeScreenStationStatus(int id)
        {
            Id = id;
            NameId = "Name" + Id;
            StatusId = "Status" + Id;
            CurrentScriptId = "CurrentScript" + Id;
        }
    }
}
