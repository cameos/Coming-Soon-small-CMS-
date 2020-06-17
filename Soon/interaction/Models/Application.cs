using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soon.interaction.Models
{
    public class Application
    {

        public Application()
        {
            this.Users = new HashSet<User>();
        }


        public Guid ApplicationId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public DateTime DatetimeAdded { get; set; }
        public string Verified { get; set; }
        public string VerifiedValue { get; set; }


        //collection
        public ICollection<User> Users { get; set; }

    }
}