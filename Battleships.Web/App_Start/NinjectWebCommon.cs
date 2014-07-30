using Battleships.Web;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: ApplicationShutdownMethod(typeof(NinjectWebCommon), "Stop")]

namespace Battleships.Web
{
    using Battleships.Core;
    using Battleships.Core.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Extensions.Conventions;
    using Ninject.Web.Common;
    using System;
    using System.Data.Entity;
    using System.Security.Principal;
    using System.Web;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }

        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind(x => x.FromAssembliesMatching("Battleships.*").SelectAllClasses().BindDefaultInterface());
            kernel.Bind<DbContext>().To<BattleshipsContext>().InRequestScope();
            kernel.Bind<UserManager<User>>().ToSelf().InRequestScope();
            kernel.Bind<IUserStore<User>>().ToConstructor(c => new UserStore<User>(c.Inject<DbContext>())).InRequestScope();
            kernel.Bind<IAuthenticationManager>().ToMethod(x => HttpContext.Current.GetOwinContext().Authentication).InRequestScope();
            kernel.Bind<IPrincipal>().ToMethod(x => HttpContext.Current.GetOwinContext().Authentication.User).InRequestScope();
        }
    }
}
