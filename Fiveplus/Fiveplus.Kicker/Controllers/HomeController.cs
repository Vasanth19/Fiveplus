using System.Web.Mvc;

namespace Fiveplus.Kicker.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }
        
        public ActionResult Landing()
        {
            ViewBag.Message = "Your Landing.";

            return View("Landing","_Layout_Landing");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
