using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soon.interaction.Models
{
    public class Articles
    {

        public Articles()
        {
            this.Comments = new HashSet<Comment>();
        }

        public Guid ArticlesId { get; set; }
        public DateTime DateAdded { get; set; }
        public string ArticleTitle { get; set; }
        public string Body { get; set; }
        public string ReadTime { get; set; }
        public byte[] Image { get; set; }
        public string ImimeType { get; set; }



        public User UserId { get; set; }
        public User User { get; set; }
       

        //collection
        public ICollection<Comment> Comments { get; set; }

    }
}