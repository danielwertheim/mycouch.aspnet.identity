using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using MyCouch;
using MyCouch.AspNet.Identity;
using Samples.Mvc5.Models;

namespace Samples.Mvc5
{
    public class MvcApplication : System.Web.HttpApplication
    {
        internal static IMyCouchClient Client { get; private set; }

        protected async void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var uri = new MyCouchUriBuilder("http://localhost:5984")
                .SetDbName("aspnet-identity")
                .SetBasicCredentials("demo", "p@ssword")
                .Build();

            Client = new MyCouchClient(uri);

            await Client.EnsureDesignDocsExists();

            //var mgr = new UserManager<ApplicationUser>(new MyCouchUserStore<ApplicationUser>(Client));
            //var usr = new ApplicationUser { UserName = "danieltest2" };
            //await mgr.CreateAsync(usr, "1q2w3e4r");
            //await mgr.AddToRoleAsync(usr.Id, "SuperHeroes");
        }
    }
}
