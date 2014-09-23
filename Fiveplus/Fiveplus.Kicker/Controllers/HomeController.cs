using System.Web.Mvc;

namespace Fiveplus.Kicker.Controllers
{
    public class HomeController : Controller
    {
        //[Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Landing()
        {
            ViewBag.Message = "Welcome to FivePlus";

            return View("Landing","_Layout_Landing");
        }

    }
}
