using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soon.interaction.Models
{
    public class Comment
    {
        public Guid CommentId { get; set; }
        public DateTime DateAdded { get; set; }
        public string Body { get; set; }


        //navigation keys
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid ArticleId { get; set; }
        public Articles Article { get; set; }
    }
}