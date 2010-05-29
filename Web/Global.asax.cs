namespace MultiTenancy.Web
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Spark;
    using Spark.Web.Mvc;

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

            // clear view engines, we don't want anything other than spark
            ViewEngines.Engines.Clear();

            // this will eventually be swapped out for a custom view engine
            // that delegates a view engine to the tenant
            var settings = new SparkSettings()
                                .SetDebug(true)
                                .SetAutomaticEncoding(true)
                                .SetDefaultLanguage(LanguageType.CSharp);
            ViewEngines.Engines.Add(new SparkViewFactory(settings));
        }
    }
}