using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Web.Security;
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
        private IArticle _art;

        public ThoughtController(IApplication app)
        {
            _app = app;
        }

        //parameterless constructor
        public ThoughtController()
        {
            _app = new ApplicationConcrete();
            _art = new ArticleConcrete();
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


            if ((session.ApplicationId == null || session.ApplicationId == Guid.Empty) || (session.UserId == null || session.UserId == Guid.Empty) || string.IsNullOrWhiteSpace(session.Username))
            {
                if (session.UserId == null || session.UserId == Guid.Empty)
                {
                    modal = "login, Please login";
                    return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
                }
            }




            modal = "add new article";
            return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
        }


        [Route("login")]
        [HttpPost]
        public ActionResult login_article(Login login)
        {

            var modal = new Object();

            if (string.IsNullOrWhiteSpace(login.opinion_login_email) || !login.opinion_login_email.Contains("@") || string.IsNullOrWhiteSpace(login.opinion_login_password))
            {
                modal = "error, something went wrong add all information";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }

            //Application application = new Application();
            var application = _app.get_by_email(login.opinion_login_email);
            if (application == null)
            {
                modal = "register and share opiniated articles";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }


            UserConcrete concrete = new UserConcrete();
            var user = concrete.get_by_application(application.ApplicationId);
            UserSession userSession = new UserSession { ApplicationId = application.ApplicationId, UserId = user.UserId, Username = user.Username };
            Session["userSession"] = userSession;
            FormsAuthentication.SetAuthCookie(application.Email, true);

            modal = "add new article";
            return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
        }


        [Route("register")]
        [HttpPost]
        public ActionResult register_article(Register register)
        {
            var modal = new Object();

            if (string.IsNullOrWhiteSpace(register.opinion_register_username) || string.IsNullOrWhiteSpace(register.opinion_register_email) || !register.opinion_register_email.Contains("@") || string.IsNullOrWhiteSpace(register.opinion_register_password) || string.IsNullOrWhiteSpace(register.opinion_register_repassword) || (register.opinion_register_password != register.opinion_register_repassword) || string.IsNullOrWhiteSpace(register.opinion_register_name) || string.IsNullOrWhiteSpace(register.opinion_register_surname))
            {
                modal = "error, something went wrong please verify every information";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }


            var salt = SecurityEncryption.generate_salt().ToString();
            var pass = SecurityEncryption.encrypt_value(salt, register.opinion_register_password);
            var verified_value = SecurityEncryption.Token().ToString();

            Application application = new Application { Phone = register.opinion_register_phone, Email = register.opinion_register_email, Password = pass, Salt = salt, VerifiedValue = verified_value, Verified = "Waiting" };
            Soon.interaction.Models.User user = new User { Username = register.opinion_register_username, Name = register.opinion_register_name, Surname = register.opinion_register_surname };
            NewApplication newApplication = new NewApplication { Application = application, User = user };


            //register this users and application
            bool flag = _app.new_application(newApplication);
            if (flag)
            {

                //message to be sent to the email
                var email_message = "<br/><img src='https://cdn.onlinewebfonts.com/svg/img_508672.png' height='30' width='30' class='rounded' style='display: inline-block;'/> <span style='font-weight:bold;font-size:1.5em;'>Message from email</span><br/><br/>";
                email_message += "<br/>Verification Code: " + application.VerifiedValue;
                email_message += "<br/><br/><i>Please enter this code to be verified in the system</i>";


                //construct mailmessage
                MailMessage message = new MailMessage("bqunta79@gmail.com", "magine20@gmail.com", "message from Bongani Qunta (Software Architect)", email_message);
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


                modal = "verify, enter verification sent to your email";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }



            modal = "error, something went wrong please verify every information";
            return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
        }

        [Route("verify")]
        [HttpPost]
        public ActionResult verify(Verification verification)
        {

            var modal = new Object();

            if (!verification.opinion_register_verify_email.Contains("@") || string.IsNullOrWhiteSpace(verification.opinion_register_verify_email) || string.IsNullOrWhiteSpace(verification.opinion_register_verify))
            {
                modal = "error, something went wrong please verify every information";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }

            var application = _app.get_by_email(verification.opinion_register_verify_email);
            if (application == null)
            {
                modal = "error, something went wrong please verify every information";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }

            var ver = verification.opinion_register_verify.Trim().TrimStart().TrimEnd();

            if (application.VerifiedValue.ToString() == ver.ToString())
            {

                //get user information
                UserConcrete concrete = new UserConcrete();
                var user = concrete.get_by_application(application.ApplicationId);
                if (user == null)
                {
                    modal = "error, we could not find this user information";
                    return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    application.Verified = "Verified";
                    bool flag = _app.update_application(application);

                    if (flag)
                    {
                        UserSession userSession = new UserSession { ApplicationId = application.ApplicationId, UserId = user.UserId, Username = user.Username };
                        Session["userSession"] = userSession;
                        FormsAuthentication.SetAuthCookie(application.Email, true);
                        modal = "add new article";
                        return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
                    }
                    else
                    {
                        modal = "error, we could not verify this information";
                        return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
                    }

                }

            }
            modal = "error, something went wrong";
            return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
        }


        [Route("article")]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult new_article(Article article)
        {
            var modal = new Object();
            if (string.IsNullOrWhiteSpace(article.opinion_title) || string.IsNullOrWhiteSpace(article.opinion_type_article) || article.opinion_image.ContentLength == 0)
            {
                modal = "error, something went wrong with your inputs we could not process this further";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }

            var session = (UserSession)Session["userSession"];
            if (session == null)
            {
                modal = "login, Please login and post";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }


            Articles articles = new Articles { ArticleTitle = article.opinion_title, Body = article.opinion_type_article, UserId = session.UserId };
            articles.ImimeType = article.opinion_image.ContentType;
            articles.Image = new byte[article.opinion_image.ContentLength];
            article.opinion_image.InputStream.Read(articles.Image, 0, article.opinion_image.ContentLength);

            //send article
            bool flag = _art.new_article(articles);
            if (flag)
            {
                modal = Url.Action("opinions", "soon");
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }

            modal = "error, something went wrong could be processed further";
            return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
        }




    }
}