using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStationStatusInfrastructure
{
    public class test
    {
        public void Test()
        {
            using (var ctx = new TestStationContext())
            {
                ctx.SaveChanges();
            }

        }
    }
}
