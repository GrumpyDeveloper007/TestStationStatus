using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStationStatusInfrastructure.Database
{
    public class TestStationContextFactory
    {
        public TestStationContext OpenSession()
        {
            return new TestStationContext();
        }
    }
}
