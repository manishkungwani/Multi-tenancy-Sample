using System.Web.Mvc;

namespace MultiTenancy.Web.Controllers
{
    public class HomeController : Controller
    {
        public virtual ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult List()
        {
            return View("List");
        }

        public ActionResult Detail()
        {
            return View("Detail");
        }

        public ActionResult Search()
        {
            return View("Search");
        }

        public ActionResult Notification()
        {
            return View("_Notification", "Ajax");
        }
    }
}
