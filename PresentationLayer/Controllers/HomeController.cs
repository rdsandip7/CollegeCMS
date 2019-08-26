using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult course()
        {
            return View();
        }

        public ActionResult features()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

    }
}
