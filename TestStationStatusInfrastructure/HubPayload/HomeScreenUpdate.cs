using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStationStatusInfrastructure.HubPayload
{
    public class HomeScreenUpdate
    {
        public List<HomeScreenStationStatus> Station { get; set; }
        
        public HomeScreenUpdate ()
        {
            Station = new List<HomeScreenStationStatus>();
        }

        public HomeScreenUpdate(int stations)
        {
            Station = new List<HomeScreenStationStatus>();
            for (int i = 0; i < stations; i++)
            {
                Station.Add(new HomeScreenStationStatus());
            }
        }

    }
}
