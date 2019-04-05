using SimonsVossSearchPrototype.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimonsVossSearchPrototype.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Search()
        {
            ViewBag.Title = "Search Prototype";

            var model = new SearchModule();

            return View(model);
        }
    }
}
