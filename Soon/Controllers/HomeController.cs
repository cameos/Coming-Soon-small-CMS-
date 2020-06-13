using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Soon.Controllers
{
    [RoutePrefix("soon")]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }


        [Route("what")]
        public ActionResult what()
        {
            return View();
        }

        [Route("opinions")]
        public ActionResult opinion()
        {
            return View();
        }

        [Route("email")]
        public ActionResult email()
        {
            return View();
        }

        [Route("playlist")]
        public ActionResult playlist()
        {
            return View();
        }

    }
}