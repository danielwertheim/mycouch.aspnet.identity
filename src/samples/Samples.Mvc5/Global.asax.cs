using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MyCouch;
using MyCouch.AspNet.Identity;

namespace Samples.Mvc5WithIdentity2
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected async void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            using (var client = CreateClient())
            {
                await client.Database.PutAsync();
                await client.EnsureDesignDocsExists();

                //var mgr = new UserManager<ApplicationUser>(new MyCouchUserStore<ApplicationUser>(client));
                //var usr = new ApplicationUser { UserName = "danieltest2" };
                //await mgr.CreateAsync(usr, "1q2w3e4r");
                //await mgr.AddToRoleAsync(usr.Id, "SuperHeroes");
            }
        }

        //protected void Application_BeginRequest()
        //{
        //    HttpContext.Current.Items["MyCouchClient"] = new MyCouchClient(CreateUri());
        //}

        //protected void Application_EndRequest()
        //{
        //    var client = HttpContext.Current.Items["MyCouchClient"] as IMyCouchClient;
        //    if (client != null)
        //        client.Dispose();
        //}

        internal static IMyCouchClient CreateClient()
        {
            return new MyCouchClient(CreateUri());
        }

        private static Uri CreateUri()
        {
            return new MyCouchUriBuilder("http://localhost:5984")
                .SetDbName("aspnet-identity")
                .SetBasicCredentials("sa", "test")
                .Build();
        }
    }
}
