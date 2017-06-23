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
        private static LocalTestDataService _SingleInstanceLocalData;
        private static LocalTestDataService _SingleInstanceLocalData2;
        private static ServerDataService _SingleInstanceServerData;
        private static RefreshClientService _SingleInstanceRefreshData;
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

        public static LocalTestDataService GetLocalTestDataService()
        {
            if (_SingleInstanceLocalData == null)
            {
                _SingleInstanceLocalData = new LocalTestDataService(GetIpAddressService());
            }
            return _SingleInstanceLocalData;
        }

        public static LocalTestDataService GetLocalTestDataService2()
        {
            if (_SingleInstanceLocalData2 == null)
            {
                _SingleInstanceLocalData2 = new LocalTestDataService(GetIpAddressService());
            }
            return _SingleInstanceLocalData2;
        }

        public static ServerDataService GetServerDataService()
        {
            if (_SingleInstanceServerData == null)
            {
                _SingleInstanceServerData = new ServerDataService(new TestStationContextFactory ());
            }
            return _SingleInstanceServerData;
        }

        public static RefreshClientService GetRefreshDataService()
        {
            if (_SingleInstanceRefreshData == null)
            {
                _SingleInstanceRefreshData = new RefreshClientService();
            }
            return _SingleInstanceRefreshData;
        }


    }
}