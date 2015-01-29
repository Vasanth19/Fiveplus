using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fiveplus.Kicker.Controllers
{
    public class GigController : Controller
    {
        //
        // GET: /Gigs/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Gigs/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Gigs/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Gigs/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
