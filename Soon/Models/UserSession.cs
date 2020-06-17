using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soon.Models
{
    public class UserSession
    {
        public Guid ApplicationId { get; set; }
        public Guid UserId { get; set; }
    }
}