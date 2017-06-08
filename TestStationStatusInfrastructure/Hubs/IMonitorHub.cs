using System.Threading.Tasks;

namespace TestStationStatusInfrastructure.Hubs
{
    public interface IMonitorHub
    {
        Task OnConnected();
        Task OnDisconnected(bool stopCalled);
        Task OnReconnected();
        void refreshPage();
    }
}