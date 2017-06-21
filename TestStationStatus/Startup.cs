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
