using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStationStatusDomain.Entities
{
    public enum TestStationStatus
    {
        Completed,
        Opened,
        Cancelled,
        Running,
        Terminated,
    }
}
