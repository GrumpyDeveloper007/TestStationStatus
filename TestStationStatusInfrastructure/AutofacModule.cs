using Autofac;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStationStatusInfrastructure.Database;
using TestStationStatusInfrastructure.Service;

namespace TestStationStatusInfrastructure
{
    [ExcludeFromCodeCoverage]
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            var assemblyToScan = new[] { ThisAssembly };

            string[] IpLoopUpsToTry = { "INDELPC217", "INDELNB352" };

            builder.RegisterType<RefreshClientService>().SingleInstance();
            builder.RegisterType<TestStationContextFactory>().SingleInstance();
            builder.RegisterType<ServerDataService>().SingleInstance();
            builder.RegisterType<LocalTestDataService>().InstancePerRequest();
            builder.Register(c => new IpAddressService(IpLoopUpsToTry));
        }
    }
}
