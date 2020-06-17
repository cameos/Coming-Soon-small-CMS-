using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Web.Security;
using System.Net;
using Soon.Models;
using Soon.interaction.Abstracts.Interfaces;
using Soon.interaction.Abstracts.Concrete;
using Soon.interaction.Models;
using Soon.interaction.Context;
using System.Data;
using System.Data.Entity;


namespace Soon.Controllers
{
    [RoutePrefix("articles")]
    public class ThoughtController : Controller
    {

        private IApplication _app;


        public ThoughtController(IApplication app)
        {
            _app = app;
        }

        //parameterless constructor
        public ThoughtController() {
            _app = new ApplicationConcrete();
        }


        // GET: Thought
        [Route("new")]
        [HttpPost]
        public ActionResult new_article(string pop_modal)
        {
            var modal = new object();
            if (string.IsNullOrWhiteSpace(pop_modal) || pop_modal != "modal")
            {
                modal = "error, something went wrong please try again";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }

            //check session
            UserSession session = new UserSession();
            session = (UserSession)Session["userSession"];
            if (session == null)
            {

                modal = "login, Please login";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }


            if ((session.ApplicationId == null || session.ApplicationId == Guid.Empty) && (session.UserId == null || session.UserId == Guid.Empty))
            {
                if (session.UserId == null || session.UserId == Guid.Empty)
                {
                    modal = "login, Please login";
                    return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
                }
            }

            return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
        }


        [Route("login")]
        [HttpPost]
        public ActionResult login_article(Login login)
        {

            var modal = new Object();

            if(string.IsNullOrWhiteSpace(login.opinion_login_email) || !login.opinion_login_email.Contains("@") || string.IsNullOrWhiteSpace(login.opinion_login_password))
            {
                modal = "error, something went wrong add all information";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }

            //Application application = new Application();
            var application = _app.get_by_email(login.opinion_login_email);
            if(application == null)
            {
                modal = "register and share opiniated articles";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }
            



            return View();
        }

    }
}