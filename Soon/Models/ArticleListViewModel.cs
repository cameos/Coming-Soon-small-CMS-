using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Soon.interaction.Models;

namespace Soon.Models
{
    public class ArticleListViewModel
    {
        public IEnumerable<Articles> Articles { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}