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
    public class ApplicationConcrete : IApplication
    {


        private SoonContext _soon;


        public bool delete_application()
        {
            throw new NotImplementedException();
        }

        public List<Application> get_all()
        {
            List<Application> apps = new List<Application>();
            using(_soon = new SoonContext())
            {
                using(var _transaction = _soon.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (!_soon.Database.Exists())
                            return apps;
                        if (_soon.Database.Connection.State == ConnectionState.Broken || _soon.Database.Connection.State == ConnectionState.Closed)
                            _soon.Database.Connection.Open();

                        apps = (from a in _soon.Application
                                select a).ToList<Application>();
                        if (apps.Count() == 0)
                            return apps;
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
            return apps;
        }

        public Application get_by_email(string email)
        {
            Application application = new Application();
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@") || email == string.Empty)
                return application;
            
            using(_soon = new SoonContext())
            {
                using(var _transaction = _soon.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (!_soon.Database.Exists())
                            return application;
                        if (_soon.Database.Connection.State == ConnectionState.Broken || _soon.Database.Connection.State == ConnectionState.Closed)
                            _soon.Database.Connection.Open();


                        application = (from a in _soon.Application
                                       where (a.Email == email)
                                       select a).FirstOrDefault<Application>();
                        if (application == null)
                            return application;
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
            return application;
        }

        public Application get_one(Guid id)
        {
            Application app = new Application();
            if (id == null || id == Guid.Empty)
                return (app = null);
            using(_soon = new SoonContext())
            {
                using(var _transaction = _soon.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (!_soon.Database.Exists())
                            return app;
                        if (_soon.Database.Connection.State == ConnectionState.Broken || _soon.Database.Connection.State == ConnectionState.Closed)
                            _soon.Database.Connection.Open();

                        app = (from a in _soon.Application
                               where (a.ApplicationId == id)
                               select a).SingleOrDefault<Application>();

                        if (app.ApplicationId == null || app.ApplicationId == Guid.Empty)
                            return (app = null);
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
            return app;
        }

        public bool update_application(Application application)
        {
            bool flag = false;
            if (application.ApplicationId == null || application.ApplicationId == Guid.Empty || application == null)
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

                        _soon.Entry<Application>(application).State = EntityState.Modified;
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

        public bool new_application(NewApplication application)
        {
            bool flag = false;
            if (application == null || application.Application == null || application.User == null)
                return flag;

            Application app = application.Application;
            if (app == null)
                return flag;

            User user = application.User;
            if (user == null)
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


                        app.DatetimeAdded = DateTime.Now;

                        _soon.Application.Add(app);
                        _soon.SaveChanges();
                        var id = app.ApplicationId;
                        if (id == null || id == Guid.Empty)
                            return (flag = false);


                        user.ApplicationId = id;
                        _soon.User.Add(user);
                        _soon.SaveChanges();
                        var id_user = user.UserId;
                        if (id_user == null || id_user == Guid.Empty)
                            return (flag = false);


                       
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

    }
}