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
    public class UserConcrete : IUser
    {

        private SoonContext _soon;


        public bool delete_suer()
        {
            throw new NotImplementedException();
        }

        public List<User> get_all()
        {
            List<User> users = new List<User>();
            using(_soon = new SoonContext())
            {
                using (var _transaction = _soon.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (!_soon.Database.Exists())
                            return users;
                        if (_soon.Database.Connection.State == ConnectionState.Broken || _soon.Database.Connection.State == ConnectionState.Closed)
                            _soon.Database.Connection.Open();

                        users = (from a in _soon.User
                              select a).ToList<User>();
                        if (users.Count() == 0)
                            return users;
                        else
                        {
                            _soon.SaveChanges();
                            _transaction.Commit();
                        }


                    }
                    catch(Exception e)
                    {
                        _transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
            return users;
        }

        public User get_one(Guid id)
        {
            User user = new User();
            if (id == null || id == Guid.Empty)
                return (user = null);
            using (_soon = new SoonContext())
            {
                using (var _transaction = _soon.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (!_soon.Database.Exists())
                            return user;
                        if (_soon.Database.Connection.State == ConnectionState.Broken || _soon.Database.Connection.State == ConnectionState.Closed)
                            _soon.Database.Connection.Open();

                        user = (from a in _soon.User
                               where (a.UserId == id)
                               select a).SingleOrDefault<User>();

                        if (user.UserId == null || user.UserId == Guid.Empty)
                            return (user = null);
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
            return user;
        }

        public bool new_application(NewApplication application)
        {
            bool flag = false;
            if (application.Application.ApplicationId != null || application.Application.ApplicationId != Guid.Empty || application.Application == null || application.User.UserId != null || application.User.UserId != Guid.Empty)
                return flag;

            Application app = application.Application;
            User user = application.User;

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

                        _soon.Application.Add(app);
                        var id = app.ApplicationId;
                        if (id == null || id == Guid.Empty)
                            return (flag = false);


                        user.ApplicationId = id;
                        _soon.User.Add(user);
                        var id_user = user.UserId;
                        if(id_user  == null || id_user == Guid.Empty)
                            return (flag = false);


                        _soon.SaveChanges();
                        _transaction.Commit();
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

        public bool update_user(User user)
        {
            bool flag = false;
            if (user.UserId == null || user.UserId == Guid.Empty || user == null)
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

                        _soon.Entry<User>(user).State = EntityState.Modified;
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