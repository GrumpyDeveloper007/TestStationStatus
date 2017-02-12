using TestStationStatusInfrastructure.Service;

namespace TestStationStatusInfrastructure
{
    public static class PoorMansIOC
    {
        private static LocalTestDataService _SingleInstanceLocalData;
        private static ServerDataService _SingleInstanceServerData;

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

    }
}
