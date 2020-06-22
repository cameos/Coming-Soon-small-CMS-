using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Soon.Models
{
    public class UpdateArticle
    {
        public string update_opinion_title { get; set; }
        public string update_opinion_minutes { get; set; }
        public string hidden_update_article { get; set; }
        public HttpPostedFileBase update_opinion_image { get; set; }


        [AllowHtml]
        public string update_opinion_type_article { get; set; }
    }
}