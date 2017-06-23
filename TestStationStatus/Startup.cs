using Microsoft.Owin;
using NLog;
using Owin;
using TestStationStatusInfrastructure.Database;

[assembly: OwinStartupAttribute(typeof(TestStationStatus.Startup))]
[assembly: OwinStartup(typeof(TestStationStatus.Startup))]
namespace TestStationStatus
{
    public partial class Startup
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private TestStationContextFactory _testStationContextFactory = new TestStationContextFactory();

        public void Configuration(IAppBuilder app)
        {
            _logger.Log(LogLevel.Info, "Application started");
            // Create instance of DB
            using (var ctx = _testStationContextFactory.OpenSession())
            {
                ctx.SaveChanges();
            }
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
