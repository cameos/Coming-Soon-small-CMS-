using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Soon.Models
{
    public class Article
    {
        public string opinion_title { get; set; }
        public string opinion_minutes { get; set; }
        public HttpPostedFileBase opinion_image { get; set; }


        [AllowHtml]
        public string opinion_type_article { get; set; }
    }
}