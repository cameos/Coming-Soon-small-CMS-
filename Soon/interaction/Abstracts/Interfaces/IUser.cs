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
        bool new_application(NewApplication application);
        bool update_user(User user);
        bool delete_suer();
        List<User> get_all();
        User get_one(Guid id);
    }
}
