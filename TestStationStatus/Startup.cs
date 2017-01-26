using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestStationStatus.Startup))]
namespace TestStationStatus
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
