using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Soon.Models;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace Soon.Controllers
{
    [RoutePrefix("soon")]
    public class HomeController : Controller
    {

        public int PageSize = 4;

        // GET: Home
        public ActionResult Index()
        {

            //if(Request.Browser.IsMobileDevice)
            //{
            //    return View();
            //}

            return View();
        }


        [Route("what")]
        public ActionResult what()
        {
            //if(Request.Browser.IsMobileDevice)
            //{
            //    return View();
            //}

            return View();
        }

        [Route("opinions")]
        public ViewResult opinion()
        {

            //if(Request.Browser.IsMobileDevice)
            //{
            //    return View();
            //}

            return View();
        }

        [Route("email")]
        public ActionResult email()
        {

            //if(Request.Browser.IsMobileDevice)
            //{
            //    return View();
            //}

            return View();
        }

        //send emails
        [Route("send")]
        [HttpPost]
        public ActionResult send_email(DeveloperEmail developer)
        {

            //if(Request.Browser.IsMobileDevice)
            //{
            //    return View();
            //}

            var error_message = new object();


            if(string.IsNullOrWhiteSpace(developer.social_name) || string.IsNullOrWhiteSpace(developer.social_surname)|| string.IsNullOrWhiteSpace(developer.social_email) || !developer.social_email.Contains("@") || string.IsNullOrWhiteSpace(developer.social_thought))
            {
                error_message = "error, Some of the fields in the form are empty please enter was needed and send email";
                return Json(error_message, "applicatiom/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }

            string name = sanitize_clean(developer.social_name);
            string surname = sanitize_clean(developer.social_surname);

            //message to be sent to the email
            var email_message = "<br/><img src='https://cdn.onlinewebfonts.com/svg/img_508672.png' height='30' width='30' class='rounded' style='display: inline-block;'/> <span style='font-weight:bold;font-size:1.5em;'>Message from email</span><br/><br/>Name: " + name + " " + surname;
            email_message += "<br/>Email: " +developer.social_email.ToString();
            email_message += "<br/><br/>" + developer.social_thought;


            //construct mailmessage
            MailMessage message = new MailMessage("bqunta79@gmail.com", "magine20@gmail.com", "message from a developer", email_message);
            message.IsBodyHtml = true;


            NetworkCredential credential = new NetworkCredential("bqunta79", "delete+92");
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.ServicePoint.MaxIdleTime = 2;
            client.Credentials = credential;


            //send email
            client.Send(message);


            error_message = Url.Action("Home", "Index");
            return Json(error_message, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
        }

        [Route("playlist")]
        public ActionResult playlist()
        {

            //if(Request.Browser.IsMobileDevice)
            //{
            //    return View();
            //}

            return View();
        }

        
        [NonAction]
        private string sanitize_clean(string dirty)
        {
            const string removeChars= " ?&^$#@!()+-,:;<>’\'-_*";
            StringBuilder sb = new StringBuilder(dirty.Length);
            foreach(char x in dirty.Where(c => !removeChars.Contains(c))) { sb.Append(x); }
            return sb.ToString();
        }

    }
}