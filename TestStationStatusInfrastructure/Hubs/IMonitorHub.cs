using System.Threading.Tasks;
using TestStationStatusInfrastructure.HubPayload;

namespace TestStationStatusInfrastructure.Hubs
{
    public interface IMonitorHub
    {
        Task OnConnected();
        Task OnDisconnected(bool stopCalled);
        Task OnReconnected();
        void refreshPage();
        void HomeUpdated(HomeScreenUpdate newStatus);
        void statusBUpdated(string status);
    }
}