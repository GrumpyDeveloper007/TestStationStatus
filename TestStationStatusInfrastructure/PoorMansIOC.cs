using System.Collections.Generic;
using TestStationStatusInfrastructure.Database;
using TestStationStatusInfrastructure.Service;

namespace TestStationStatusInfrastructure
{
    /// <summary>
    /// My way of using the concept of IOC/dependency injection without having a IOC container.
    /// I could just go for any standard container, but I would like to keep the project as simple 
    /// as possible and hopefully learn something along the way
    /// </summary>
    public static class PoorMansIOC
    {
        private static List<LocalTestDataService> _LocalData=new List<LocalTestDataService> ();
        private static IpAddressService _SingleInstanceIpAddressService;

        private static string[] IpLoopUpsToTry={ "INDELPC217", "INDELNB352" };

        public static IpAddressService GetIpAddressService()
        {
            if (_SingleInstanceIpAddressService == null)
            {
                _SingleInstanceIpAddressService = new IpAddressService(IpLoopUpsToTry);
            }
            return _SingleInstanceIpAddressService;
        }

        public static List<LocalTestDataService> GetLocalTestDataServices()
        {
            return _LocalData;
        }

        public static LocalTestDataService GetLocalTestDataService(int index)
        {
            while (_LocalData.Count <= index)
            {
                _LocalData.Add(new LocalTestDataService(GetIpAddressService()));
            }
            return _LocalData[index];
        }
    }
}