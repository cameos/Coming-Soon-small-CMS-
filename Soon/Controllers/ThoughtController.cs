using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Soon.Controllers
{
    [RoutePrefix("articles")]
    public class ThoughtController : Controller
    {
        // GET: Thought
        [Route("new")]
        [HttpGet]
        public ActionResult new_article()
        {
            return View();
        }
    }
}