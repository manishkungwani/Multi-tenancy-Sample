namespace MultiTenancy.Web
{
    using System.IO;
    using System.Web.Mvc;
    using System.Web.Routing;
    using MultiTenancy.Core;
    using StructureMap;

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        protected void Application_Start()
        {
            // I'm not using areas... no need to register.
            // AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);

            // create a container just to pull in tenants
            var topContainer = new Container();
            topContainer.Configure(config =>
            {
                config.Scan(scanner =>
                {
                    // scan the tenants folder
                    // (For some reason just passing "Tenants" doesn't seem to work, which it should according to the docs)
                    scanner.AssembliesFromPath(Path.Combine(Server.MapPath("~/"), "Tenants"));

                    // pull in all the tenant types
                    scanner.AddAllTypesOf<IApplicationTenant>();
                });
            });

            // create selectors
            var tenantSelector = new DefaultTenantSelector(topContainer.GetAllInstances<IApplicationTenant>());
            var containerSelector = new TenantContainerResolver(tenantSelector);
            
            // clear view engines, we don't want anything other than spark
            ViewEngines.Engines.Clear();
            // set view engine
            ViewEngines.Engines.Add(new TenantViewEngine(tenantSelector));

            // set controller factory
            ControllerBuilder.Current.SetControllerFactory(new ContainerControllerFactory(containerSelector));
        }
    }
}