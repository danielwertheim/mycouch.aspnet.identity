using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MyCouch;
using MyCouch.AspNet.Identity;

namespace Samples.Mvc5
{
    public class MvcApplication : System.Web.HttpApplication
    {
        internal static IClient Client { get; private set; }

        protected async void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var uri = new MyCouchUriBuilder("http://localhost:5984")
                .SetDbName("aspnet_identity")
                .SetBasicCredentials("demo", "p@ssword")
                .Build();

            Client = new Client(uri);

            await Client.EnsureDesignDocsExists();
        }
    }
}
