using System.Web.Mvc;

namespace MultiTenancy.Tenants.Sample1.Controllers
{
    public class HomeController : MultiTenancy.Web.Controllers.HomeController
    {
        public override ActionResult Index()
        {
            // Set a breakpoint here to see that this is the "HomeController" that is requested
            return base.Index();
        }
    }
}