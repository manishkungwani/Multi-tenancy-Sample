namespace MultiTenancy.Tenants.Sample1
{
    using System.Web.Mvc;
    using MultiTenancy.Core;
    using MultiTenancy.Web.Controllers;
    using StructureMap;

    /// <summary>
    /// Tenant for "Sample1" Tenant
    /// </summary>
    public class SampleTenant : AbstractApplicationTenant
    {
        /// <summary>
        /// Initializes a new instance of the SampleTenant class
        /// </summary>
        public SampleTenant()
        {
            // setup view engine
            ViewEngine = DetermineViewEngine();

            // setup dependency container
            DependencyContainer = new Container();
            DependencyContainer.Configure(config =>
            {
                // this will be changed and simplified!
                config.AddType(typeof(IController), typeof(HomeController));
                config.AddType(typeof(IController), typeof(AccountController));

                config.For<IApplicationTenant>().Singleton().Use(this);
            });

            ApplicationName = "Sample 1";
            MinifyJavaScript = true;
            MinifyCSS = true;
            EnabledFeatures = null;
            UrlPaths = new[] { "http://localhost:3454/" };
        }
    }
}