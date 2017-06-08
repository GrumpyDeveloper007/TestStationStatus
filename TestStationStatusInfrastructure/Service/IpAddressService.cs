using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestStationStatusInfrastructure.Service
{
    public class IpAddressService
    {
        public static string DetermineComputerName(string IP)
        {
            string[] addressesToTry = {IP, "INDELPC217", "INDELNB352" };
            int i = 0;
            IPHostEntry GetIPHost = null;
            bool found = false;

            while (found == false && i < addressesToTry.Count())
            {
                string addressToCheck = addressesToTry[i];
                i++;
                try
                {
                    GetIPHost = Dns.GetHostEntry(addressToCheck);
                    if (i==1)
                    {
                        found = true;
                    }
                }
                catch (Exception ex)
                {
                    // Ignore DNS errors
                }

                if (GetIPHost != null)
                {
                    // Try the hacky way to get HostNames
                    foreach (var ip in GetIPHost.AddressList)
                    {
                        if (ip.ToString() == IP)
                        {
                            found = true;
                        }
                    }
                }
            }

            if (found == true)
            {
                List<string> compName = GetIPHost.HostName.ToString().Split('.').ToList();
                return compName.First();
            }
            else
            {
                return IP;
            }
        }

    }
}
