namespace Battleships.Web
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    [ExcludeFromCodeCoverage]
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            RouteTable.Routes.MapMvcAttributeRoutes();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}