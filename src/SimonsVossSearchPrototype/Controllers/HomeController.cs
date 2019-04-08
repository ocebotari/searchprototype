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
            ViewBag.Title = "Search";

            return View();
        }

        [Route("buildings")]
        public ActionResult SearchBuildings()
        {
            ViewBag.Title = "Search Buildings";

            var model = new SearchModule();

            return View("SearchBuildings", model);
        }

        [Route("locks")]
        public ActionResult SearchLocks()
        {
            ViewBag.Title = "Search Locks";

            var model = new SearchModule();

            return View(model);
        }

        [Route("groups")]
        public ActionResult SearchGroups()
        {
            ViewBag.Title = "Search Groups";

            var model = new SearchModule();

            return View(model);
        }
        [Route("medias")]
        public ActionResult SearchMedias()
        {
            ViewBag.Title = "Search Medias";

            var model = new SearchModule();

            return View(model);
        }

    }
}
