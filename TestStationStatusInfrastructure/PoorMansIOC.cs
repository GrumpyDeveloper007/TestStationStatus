using TestStationStatusInfrastructure.Service;

namespace TestStationStatusInfrastructure
{
    public static class PoorMansIOC
    {
        private static LocalTestDataService _SingleInstanceLocalData;
        private static LocalTestDataService _SingleInstanceLocalData2;
        private static ServerDataService _SingleInstanceServerData;
        private static RefreshClientService _SingleInstanceRefreshData;

        public static LocalTestDataService GetLocalTestDataService()
        {
            if (_SingleInstanceLocalData==null )
            {
                _SingleInstanceLocalData = new LocalTestDataService();
            }
            return _SingleInstanceLocalData;
        }

        public static LocalTestDataService GetLocalTestDataService2()
        {
            if (_SingleInstanceLocalData2 == null)
            {
                _SingleInstanceLocalData2 = new LocalTestDataService();
            }
            return _SingleInstanceLocalData2;
        }

        public static ServerDataService GetServerDataService()
        {
            if (_SingleInstanceServerData == null)
            {
                _SingleInstanceServerData = new ServerDataService();
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
