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
        public int PageSize = 3;
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
            AuthAccess modal = new AuthAccess();
            if (string.IsNullOrWhiteSpace(pop_modal) || pop_modal != "modal")
            {
                modal.modal = "error, something went wrong please try again";
                modal.available = "none";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }

            //check session
            UserSession session = new UserSession();
            session = (UserSession)Session["userSession"];
            if (session == null)
            {

                modal.modal = "login, Please login";
                modal.available = "none";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }


            if ((session.ApplicationId == null || session.ApplicationId == Guid.Empty) || (session.UserId == null || session.UserId == Guid.Empty) || string.IsNullOrWhiteSpace(session.Username))
            {
                if (session.UserId == null || session.UserId == Guid.Empty)
                {
                    modal.modal = "login, Please login";
                    modal.available = "none";
                    return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
                }
            }




            modal.modal = "add new article";
            modal.available = "none";
            return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
        }


        [Route("login")]
        [HttpPost]
        public ActionResult login_article(Login login)
        {

            AuthAccess modal = new AuthAccess();

            if (string.IsNullOrWhiteSpace(login.opinion_login_email) || !login.opinion_login_email.Contains("@") || string.IsNullOrWhiteSpace(login.opinion_login_password))
            {
                modal.modal = "error, something went wrong add all information";
                modal.available = "none";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }


            //Application application = new Application();
            var application = _app.get_by_email(login.opinion_login_email);
            if (application == null)
            {
                modal.modal = "register and share opiniated articles";
                modal.available = "none";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }


            UserConcrete concrete = new UserConcrete();
            var user = concrete.get_by_application(application.ApplicationId);
            if (user == null)
            {
                modal.modal = "register and share opiniated articles";
                modal.available = "none";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }

            var pass = SecurityEncryption.encrypt_value(application.Salt, login.opinion_login_password);
            if (pass == application.Password)
            {
                UserSession userSession = new UserSession { ApplicationId = application.ApplicationId, UserId = user.UserId, Username = user.Username };
                Session["userSession"] = userSession;
                FormsAuthentication.SetAuthCookie(application.Email, true);

                modal.modal = "add new article";
                modal.available = "yes";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }

            modal.modal = "error, something went wrong and could not be processed further";
            modal.available = "none";
            return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);

        }


        [Route("register")]
        [HttpPost]
        public ActionResult register_article(Register register)
        {
            AuthAccess modal = new AuthAccess();

            if (string.IsNullOrWhiteSpace(register.opinion_register_username) || string.IsNullOrWhiteSpace(register.opinion_register_email) || !register.opinion_register_email.Contains("@") || string.IsNullOrWhiteSpace(register.opinion_register_password) || string.IsNullOrWhiteSpace(register.opinion_register_repassword) || (register.opinion_register_password != register.opinion_register_repassword) || string.IsNullOrWhiteSpace(register.opinion_register_name) || string.IsNullOrWhiteSpace(register.opinion_register_surname))
            {
                modal.modal= "error, something went wrong please verify every information";
                modal.available = "none";
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
                var email_message = "<br/><img src='https://cdn.onlinewebfonts.com/svg/img_508672.png' height='30' width='30' class='rounded' style='display: inline-block;'/> <span style='font-weight:bold;font-size:1.5em;'>Message from (Software Architect)</span><br/><br/>";
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


                modal.modal = "verify, enter verification sent to your email";
                modal.available = "none";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }



            modal.modal = "error, something went wrong please verify every information";
            modal.available = "none";
            return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
        }

        [Route("verify")]
        [HttpPost]
        public ActionResult verify(Verification verification)
        {

            AuthAccess modal = new AuthAccess();

            if (!verification.opinion_register_verify_email.Contains("@") || string.IsNullOrWhiteSpace(verification.opinion_register_verify_email) || string.IsNullOrWhiteSpace(verification.opinion_register_verify))
            {
                modal.modal = "error, something went wrong please verify every information";
                modal.available = "none";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }

            var application = _app.get_by_email(verification.opinion_register_verify_email);
            if (application == null)
            {
                modal.modal = "error, something went wrong please enter valid email";
                modal.available = "none";
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
                    modal.modal = "error, we could not find this user information";
                    modal.available = "none";
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
                        modal.modal = "add new article";
                        modal.available = "yes";
                        return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
                    }
                    else
                    {
                        modal.modal = "error, we could not verify this information";
                        modal.available = "none";
                        return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
                    }

                }

            }
            modal.modal = "error, something went wrong";
            modal.available = "none";
            return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
        }


        [Route("article")]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult new_article(Article article)
        {
            AuthAccess modal = new AuthAccess();

            var session = (UserSession)Session["userSession"];
            if (session == null)
            {
                modal.modal = "login, Please login and post";
                modal.available = "none";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }


            if (string.IsNullOrWhiteSpace(article.opinion_title) || string.IsNullOrWhiteSpace(article.opinion_type_article) || article.opinion_image.ContentLength == 0)
            {
                modal.modal = "error, something went wrong with your inputs we could not process this further";
                modal.available = "yes";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }

           

            Articles articles = new Articles { ArticleTitle = article.opinion_title, Body = article.opinion_type_article, UserId = session.UserId };
            if (string.IsNullOrWhiteSpace(article.opinion_minutes))
            {
                articles.ReadTime = "3 Mins";
            }
            else
            {
                articles.ReadTime = article.opinion_minutes+" Mins";
            }


            articles.ImimeType = article.opinion_image.ContentType;
            articles.Image = new byte[article.opinion_image.ContentLength];
            article.opinion_image.InputStream.Read(articles.Image, 0, article.opinion_image.ContentLength);


            //send article
            bool flag = _art.new_article(articles);
            if (flag)
            {
                modal.modal = Url.Action("opinions", "soon");
                modal.available = "yes";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }

            modal.modal = "error, something went wrong could be processed further";
            modal.available = "yes";
            return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
        }


        [Route("file")]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public FileContentResult get_image(Guid imageId)
        {
            //get content from the web service
            List<Articles> articles = new List<Articles>();
            

            var article = articles.FirstOrDefault(x => x.ArticlesId == imageId);

            if (article != null)
            {
                return File(article.Image, article.ImimeType);
            }
            else
            {
                return null;
            }

        }


        [Route("read")]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult read_article(Guid articleId)
        {

            
            ReadArticle read = new ReadArticle();

            AuthAccess modal = new AuthAccess();
            if(articleId == null || articleId == Guid.Empty)
            {
                modal.modal = "error, something went wrong please verify every information";
                modal.available = "none";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }


            read.Articles = _art.get_one(articleId);
            if(read.Articles == null)
            {
                modal.modal = "error, we could not get this article";
                modal.available = "none";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }


            UserConcrete userConcrete = new UserConcrete();
            read.User = userConcrete.get_one(read.Articles.UserId);
            if(read.User == null)
            {
                modal.modal = "error, we could not get user who wrote this article";
                modal.available = "none";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }

            return View(read);
        }


        [Route("signout")]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult signout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();         
            return RedirectToActionPermanent("opinions", "soon");
        }

        [Route("all")]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult all_user_articles(int page = 1)
        {

            AuthAccess modal = new AuthAccess();


            var session = (UserSession)Session["userSession"];
            if (session == null || Session.Count == 0)
            {
                modal.modal = "login, Please login and post";
                modal.available = "none";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }

            Guid id = session.UserId;
            ArticleListViewModel model = new ArticleListViewModel
            {
                Articles = _art.get_by_user_id(id)
                .OrderBy(a => a.ArticlesId)
                .Skip((page - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _art.get_all().Count()
                }
            };

            return View(model);
        }

        [Route("delete")]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult delete_article(Guid articleDelete)
        {
            
            if(articleDelete == null || articleDelete == Guid.Empty)
            {
                return View("user_error_page");
            }



            bool flag = _art.delete_article(articleDelete);
            if (flag)
            {
                return RedirectToAction("all", "articles");
            }
            else
            {
                return View("user_error_page");
            }          

        }


        [Route("update")]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult update_article(Guid articleUpdate)
        {
            ReadArticle read = new ReadArticle();

            AuthAccess modal = new AuthAccess();
            if (articleUpdate == null || articleUpdate == Guid.Empty)
            {
                return View("user_error_page");
            }


            read.Articles = _art.get_one(articleUpdate);
            if (read.Articles == null)
            {
                return View("user_error_page");
            }


            UserConcrete userConcrete = new UserConcrete();
            read.User = userConcrete.get_one(read.Articles.UserId);
            if (read.User == null)
            {
                return View("user_error_page");
            }

            return View(read);
        }

        [Route("modify")]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult modify_article(UpdateArticle article)
        {
            AuthAccess modal = new AuthAccess();

            if (string.IsNullOrWhiteSpace(article.update_opinion_type_article) || string.IsNullOrWhiteSpace(article.hidden_update_article))
            {
                modal.modal = "error, something went wrong with your inputs we could not process this further";
                modal.available = "yes";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }

            Guid id = Guid.Parse(article.hidden_update_article);
            Articles articles = _art.get_one(id);
            if (articles == null)
            {
                modal.modal = "error, we could not get this article";
                modal.available = "none";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }

            if (!string.IsNullOrWhiteSpace(article.update_opinion_title))
            {
                articles.ArticleTitle = article.update_opinion_title;
            }
            if (!string.IsNullOrWhiteSpace(article.update_opinion_minutes))
            {
                articles.ReadTime = article.update_opinion_minutes;
            }
            if (!string.IsNullOrWhiteSpace(article.update_opinion_type_article))
            {
                articles.Body = article.update_opinion_type_article;
            }
            if(article.update_opinion_image != null)
            {
                articles.ImimeType = article.update_opinion_image.ContentType;
                articles.Image = new byte[article.update_opinion_image.ContentLength];
                article.update_opinion_image.InputStream.Read(articles.Image, 0, article.update_opinion_image.ContentLength);
            }

            //send article
            bool flag = _art.update_article(articles);
            if (flag)
            {
                modal.modal = Url.Action("all", "articles");
                modal.available = "yes";
                return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }

            modal.modal = "error, something went wrong could be processed further";
            modal.available = "yes";
            return Json(modal, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);

        }


    }
}