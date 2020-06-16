using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soon.interaction.Models;

namespace Soon.interaction.Abstracts.Interfaces
{
    public interface IComment
    {
        bool new_comment(Comment comment);
        bool update_comment(Comment comment);
        bool delete_comment();
        List<Comment> get_all();
        Comment get_one(Guid id);
    }
}
