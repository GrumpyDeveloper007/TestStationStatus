using Microsoft.Owin;
using NLog;
using Owin;
using TestStationStatusInfrastructure;

[assembly: OwinStartupAttribute(typeof(TestStationStatus.Startup))]
[assembly: OwinStartup(typeof(TestStationStatus.Startup))]
namespace TestStationStatus
{
    public partial class Startup
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public void Configuration(IAppBuilder app)
        {
            _logger.Log(LogLevel.Info, "Application started");
            // Create instance of DB
            using (var ctx = new TestStationContext())
            {
                ctx.SaveChanges();
            }
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
