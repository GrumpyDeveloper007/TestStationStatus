using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestStationStatusInfrastructure.Service
{
    /// <summary>
    /// Service to translate IP to Name etc
    /// </summary>
    public class IpAddressService
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private List<string> _lookupsToTry;

        public IpAddressService(string[] lookupsToTry)
        {
            _lookupsToTry = new List<string>();
            _lookupsToTry.AddRange(lookupsToTry);
        }

        public void UpdateLoopUpsToTry(string[] lookupsToTry)
        {
            if (lookupsToTry.Count() > 0)
            {
                _lookupsToTry = new List<string>(lookupsToTry);
            }
        }

        public string DetermineComputerName(string IP)
        {
            List<string> addressesToTry = new List<string>();
            addressesToTry.Add(IP);
            addressesToTry.AddRange(_lookupsToTry);
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
                    if (i == 1)
                    {
                        found = true;
                    }
                }
                catch (Exception ex)
                {
                    // Ignore DNS errors
                    _logger.Log(LogLevel.Info, ex);
                }

                if (GetIPHost != null)
                {
                    // Because reverse DNS doe not work for the computers based off site we try a normal DNS lookup
                    // for each possible user
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