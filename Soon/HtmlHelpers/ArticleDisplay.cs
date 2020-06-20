using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace Soon.HtmlHelpers
{
    public static class ArticleDisplay
    {
        public static MvcHtmlString display_body(this HtmlHelper html, string content)
        {
            content = HttpUtility.HtmlDecode(content);
            content = Regex.Replace(content, "<.*?>", string.Empty);
            if(content.Length > 50)
            {
                content = content.Substring(0, 50) + ".....".ToString();
            }
            return new MvcHtmlString(content);
        }
    }
}