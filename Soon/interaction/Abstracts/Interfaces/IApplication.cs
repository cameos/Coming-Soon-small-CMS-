using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soon.interaction.Models;

namespace Soon.interaction.Abstracts.Interfaces
{
    public interface IApplication
    {


        bool update_application(Application application);
        bool delete_application();
        List<Application> get_all();
        Application get_one(Guid id);
        Application get_by_email(string email);
        bool new_application(NewApplication application);
    }
}
