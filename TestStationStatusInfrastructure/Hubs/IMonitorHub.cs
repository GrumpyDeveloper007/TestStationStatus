using System.Threading.Tasks;

namespace TestStationStatusInfrastructure.Hubs
{
    public interface IMonitorHub
    {
        Task OnConnected();
        Task OnDisconnected(bool stopCalled);
        Task OnReconnected();
        void refreshPage();
        void Updated(string statusA, string currentScriptA, string statusB, string currentScriptB
           , string statusA2, string currentScriptA2, string statusB2, string currentScriptB2);
        void statusBUpdated(string status);
    }
}