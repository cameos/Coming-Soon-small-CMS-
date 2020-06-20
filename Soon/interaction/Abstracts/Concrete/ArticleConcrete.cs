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
    public class ArticleConcrete : IArticle
    {


        private SoonContext _soon;

        public bool delete_article()
        {
            throw new NotImplementedException();
        }

        public List<Articles> get_all()
        {
            List<Articles> articles = new List<Articles>();
            using (_soon = new SoonContext())
            {
                using (var _transaction = _soon.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (!_soon.Database.Exists())
                            return articles;
                        if (_soon.Database.Connection.State == ConnectionState.Broken || _soon.Database.Connection.State == ConnectionState.Closed)
                            _soon.Database.Connection.Open();

                        articles = (from a in _soon.Article
                                 select a).ToList<Articles>();
                        if (articles.Count() == 0)
                            return articles;
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
            return articles;
        }

        public Articles get_one(Guid id)
        {
            Articles article = new Articles();
            if (id == null || id == Guid.Empty)
                return (article = null);
            using (_soon = new SoonContext())
            {
                using (var _transaction = _soon.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (!_soon.Database.Exists())
                            return article;
                        if (_soon.Database.Connection.State == ConnectionState.Broken || _soon.Database.Connection.State == ConnectionState.Closed)
                            _soon.Database.Connection.Open();

                        article = (from a in _soon.Article
                                where (a.ArticlesId == id)
                                select a).SingleOrDefault<Articles>();

                        if (article.ArticlesId == null || article.ArticlesId == Guid.Empty)
                            return (article = null);
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
            return article;
        }

        public bool new_article(Articles article)
        {
            bool flag = false;
            if (article == null || string.IsNullOrWhiteSpace(article.ArticleTitle) || string.IsNullOrWhiteSpace(article.Body) || string.IsNullOrWhiteSpace(article.ArticleTitle))
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


                        article.ReadTime = "3 Mins";
                        article.DateAdded = DateTime.Now;

                        _soon.Article.Add(article);
                        _soon.SaveChanges();
                        var id = article.ArticlesId;
                        if (id == null || id == Guid.Empty)
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

        public bool update_article(Articles article)
        {
            bool flag = false;
            if (article.ArticlesId == null || article.ArticlesId == Guid.Empty || article == null)
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

                        _soon.Entry<Articles>(article).State = EntityState.Modified;
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