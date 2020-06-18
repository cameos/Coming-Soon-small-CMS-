using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soon.interaction.Models;

namespace Soon.interaction.Abstracts.Interfaces
{
    public interface IUser
    {
        bool update_user(User user);
        bool delete_suer();
        List<User> get_all();
        User get_one(Guid id);
        User get_by_application(Guid id);
    }
}
