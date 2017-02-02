using Microsoft.Owin;
using Owin;
using TestStationStatusInfrastructure;

[assembly: OwinStartupAttribute(typeof(TestStationStatus.Startup))]
[assembly: OwinStartup(typeof(TestStationStatus.Startup))]
namespace TestStationStatus
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            test database = new test();
            database.Test();
            ConfigureAuth(app);
        }
    }
}
