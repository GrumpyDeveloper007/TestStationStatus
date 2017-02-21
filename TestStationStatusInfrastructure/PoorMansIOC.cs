using TestStationStatusInfrastructure.Service;

namespace TestStationStatusInfrastructure
{
    public static class PoorMansIOC
    {
        private static LocalTestDataService _SingleInstanceLocalData;
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
            if (_SingleInstanceServerData == null)
            {
                _SingleInstanceRefreshData = new RefreshClientService();
            }
            return _SingleInstanceRefreshData;
        }
        

    }
}
