using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TEAM5OCOS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "TEAM5OCOS about page";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Page.";

            return View();
        }

        public ActionResult Testimonials()
        {
            ViewBag.Message = "Testimonial page.";

            return View();
        }

        public ActionResult Chat()
        {
            ViewBag.Message = "Chat page.";
            return View();
        }
    }
}