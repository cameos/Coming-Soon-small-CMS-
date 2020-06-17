using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Ninject;
using Ninject.Extensions.ChildKernel;
using System.Configuration;
using System.Web.Routing;
using Soon.interaction.Abstracts.Interfaces;
using Soon.interaction.Abstracts.Concrete;
using Soon.interaction.Models;

namespace Soon.Infrastructure
{
    public class CustomDependency : IDependencyResolver
    {
        private IKernel kernel;

        public CustomDependency() : this(new StandardKernel()) { }

        public CustomDependency(IKernel ninjectKernel, bool scope = false)
        {
            kernel = ninjectKernel;
            if (!scope)
                AddBindings(kernel);

        }

        public IDependencyScope BeginScope()
        {
            return new CustomDependency(AddRequestBindings(new ChildKernel(kernel)), true);
        }


        public void Dispose()
        {

        }


        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings(IKernel kernel)
        {
            kernel.Bind<IApplication>().To<ApplicationConcrete>();
            kernel.Bind<IUser>().To<UserConcrete>();
            kernel.Bind<IArticle>().To<ArticleConcrete>();
            kernel.Bind<IComment>().To<CommentConcrete>();
        }

        private IKernel AddRequestBindings(IKernel kernel)
        {
            kernel.Bind<IApplication>().To<ApplicationConcrete>();
            kernel.Bind<IUser>().To<UserConcrete>();
            kernel.Bind<IArticle>().To<ArticleConcrete>();
            kernel.Bind<IComment>().To<CommentConcrete>();

            return kernel;
        }

    }
}