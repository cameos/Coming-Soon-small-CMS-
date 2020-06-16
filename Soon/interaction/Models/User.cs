using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soon.interaction.Models
{
    public class User
    {

        public User()
        {
            this.Articles = new HashSet<Articles>();
            this.Comments = new HashSet<Comment>();
        }



        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }


        //navigation properties
        public Guid ApplicationId { get; set; }
        public Application Application { get; set; }


        //Collection
        public ICollection<Articles> Articles { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}