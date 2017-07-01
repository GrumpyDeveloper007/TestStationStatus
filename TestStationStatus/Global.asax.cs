using Autofac;
using Autofac.Integration.Mvc;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TestStationStatusInfrastructure;
using TestStationStatusInfrastructure.Database;
using TestStationStatusInfrastructure.Service;

namespace TestStationStatus
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private IEnumerable<Assembly> FindAssembliesToScan()
        {
            string assemblyScannerPattern = @"TestStation*.dll";

            //Get all assemblies from the current working directory, where the assembly name fits the above scanner pattern
            IEnumerable<string> names = Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory,
                                                                 assemblyScannerPattern, SearchOption.AllDirectories)
                                                 //Make sure we apply the distinct, as we only want to load, scan and register each assembly once
                                                 .Distinct(new AssemblyNameComparer());

            //Now load the assembly, and return them
            return names.Select(Assembly.LoadFrom);
        }

        /// <summary>
        ///     Comparer to check if the filename of the assemblies are the same regardless of their directory
        /// </summary>
        private class AssemblyNameComparer : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                string xName = Path.GetFileName(x);
                string yName = Path.GetFileName(y);

                return xName.Equals(yName, StringComparison.CurrentCultureIgnoreCase);
            }

            public int GetHashCode(string obj)
            {
                return Path.GetFileName(obj).GetHashCode();
            }
        }

        protected void Application_Start()
        {

            var builder = new ContainerBuilder();

            List<Assembly> assemblies = FindAssembliesToScan().ToList();

            builder.RegisterAssemblyModules(assemblies.ToArray());

//            builder.RegisterModule<AutofacModule>();
  //          builder.Register<RefreshClientService>(b => new RefreshClientService ());
//            builder.Register<TestStationContextFactory>(b => new TestStationContextFactory());
            //builder.RegisterType<RefreshClientService>().SingleInstance ();
            //builder.RegisterType<TestStationContextFactory>().SingleInstance ();
            //builder.RegisterType<ServerDataService>().SingleInstance();

            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();

            // OPTIONAL: Enable action method parameter injection (RARE).
            //builder.InjectActionInvoker();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();

            //RouteTable.Routes.MapHubs();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DataTables.AspNet.Mvc5.Configuration.RegisterDataTables();

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

            // Get the exception object.
            Exception exc = Server.GetLastError();
            _logger.Fatal(exc);
            //Server.ClearError();
            //Response.Redirect("/Home/Error");
        }
    }
}
