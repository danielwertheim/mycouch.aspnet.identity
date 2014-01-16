using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MyCouch;

namespace Samples.Mvc5
{
    public class MvcApplication : System.Web.HttpApplication
    {
        internal static IClient Client { get; private set; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Client = new Client("http://localhost:5984/aspnet_identity");
        }
    }
}
