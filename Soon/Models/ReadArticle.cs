using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Soon.interaction.Models;

namespace Soon.Models
{
    public class ReadArticle
    {
        public Articles Articles { get; set; }
        public User User { get; set; }
    }
}