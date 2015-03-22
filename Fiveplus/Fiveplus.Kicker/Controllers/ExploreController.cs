using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fiveplus.Kicker.Controllers
{
    //[RoutePrefix("Explore")]
    //[Route]
    public class ExploreController : Controller
    {
        [Route("Gig")]
        public ActionResult Gig()
        {
            return View("~/Views/Explore/Gig.cshtml");
        }

        
        [Route("Discover")]
        public ActionResult Discover()
        {
            return View("~/Views/Explore/Discover.cshtml");
        }

      //  [Route("~/")]
        [Route]
        [Route("Home")]
        public ActionResult Home()
        {
            return View("~/Views/Explore/Home.cshtml");
        }

        [Route("Landing")]
        public ActionResult Landing()
        {
            return View("~/Views/Explore/Landing.cshtml", "_Layout_Landing");
        }

        [Route("Terms")]
        public ActionResult Terms()
        {
            return View("~/Views/Explore/Terms.cshtml");
        }

    }
}
