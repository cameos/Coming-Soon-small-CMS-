using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Soon.interaction.Models;
using Soon.interaction.Abstracts.Interfaces;
using Soon.interaction.Context;
using System.Data;
using System.Data.Entity;

namespace Soon.interaction.Abstracts.Concrete
{
    public class CommentConcrete : IComment
    {

        private SoonContext _soon;

        public bool delete_comment()
        {
            throw new NotImplementedException();
        }

        public List<Comment> get_all()
        {
            List<Comment> comments = new List<Comment>();
            using (_soon = new SoonContext())
            {
                using (var _transaction = _soon.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (!_soon.Database.Exists())
                            return comments;
                        if (_soon.Database.Connection.State == ConnectionState.Broken || _soon.Database.Connection.State == ConnectionState.Closed)
                            _soon.Database.Connection.Open();

                        comments = (from a in _soon.Comment
                                    select a).ToList<Comment>();
                        if (comments.Count() == 0)
                            return comments;
                        else
                        {
                            _soon.SaveChanges();
                            _transaction.Commit();
                        }


                    }
                    catch (Exception e)
                    {
                        _transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
            return comments;
        }

        public Comment get_one(Guid id)
        {
            Comment comment = new Comment();
            if (id == null || id == Guid.Empty)
                return (comment = null);
            using (_soon = new SoonContext())
            {
                using (var _transaction = _soon.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (!_soon.Database.Exists())
                            return comment;
                        if (_soon.Database.Connection.State == ConnectionState.Broken || _soon.Database.Connection.State == ConnectionState.Closed)
                            _soon.Database.Connection.Open();

                        comment = (from a in _soon.Comment
                                   where (a.CommentId == id)
                                   select a).SingleOrDefault<Comment>();

                        if (comment.CommentId == null || comment.CommentId == Guid.Empty)
                            return (comment = null);
                        else
                        {
                            _soon.SaveChanges();
                            _transaction.Commit();
                        }

                    }
                    catch (Exception e)
                    {
                        _transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
            return comment;
        }

        public bool new_comment(Comment comment)
        {
            bool flag = false;
            if (comment.CommentId != null || comment.CommentId != Guid.Empty || comment == null)
                return flag;
            using(_soon = new SoonContext())
            {
                using(var _transaction = _soon.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (!_soon.Database.Exists())
                            return flag;
                        if (_soon.Database.Connection.State == ConnectionState.Broken || _soon.Database.Connection.State == ConnectionState.Closed)
                            _soon.Database.Connection.Open();

                        _soon.Comment.Add(comment);
                        var id = comment.CommentId;
                        if (id == null || id == Guid.Empty)
                            return (flag = false);

                        _soon.SaveChanges();
                        _transaction.Commit();
                        flag = true;

                    }
                    catch(Exception e)
                    {
                        _transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
            return flag;
        }

        public bool update_comment(Comment comment)
        {
            bool flag = false;
            if (comment.CommentId == null || comment.CommentId == Guid.Empty || comment == null)
                return flag;

            using (_soon = new SoonContext())
            {
                using (var _transaction = _soon.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (!_soon.Database.Exists())
                            return flag;
                        if (_soon.Database.Connection.State == ConnectionState.Broken || _soon.Database.Connection.State == ConnectionState.Closed)
                            _soon.Database.Connection.Open();

                        _soon.Entry<Comment>(comment).State = EntityState.Modified;
                        _soon.SaveChanges();
                        _transaction.Rollback();
                        flag = true;


                    }
                    catch (Exception e)
                    {
                        _transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
            return flag;
        }
    }
}