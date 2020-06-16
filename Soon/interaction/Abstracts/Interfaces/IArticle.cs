using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soon.interaction.Models;

namespace Soon.interaction.Abstracts.Interfaces
{
    public interface IArticle
    {
        bool new_article(Articles article);
        bool update_article(Articles article);
        bool delete_article();
        List<Articles> get_all();
        Articles get_one(Guid id);
    }
}
