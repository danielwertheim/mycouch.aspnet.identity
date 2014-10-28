using System;
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
        protected async void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Demo bootstrap of ensuring database existance
            //and view existance.
            //NOTE! Only needed once per DB and should
            //use some sort of bootstrap account.
            using (var store = CreateStore())
            {
                //Idem potent
                await store.Client.Database.PutAsync();

                //Create secodnary indexes a.k.a views
                await store.Client.EnsureAspNetIdentityDesignDocsExists();

                //If you want to force a restore if the original shipped view
                //await client.EnsureCleanAspNetIdentityDesignDocsExists();
            }
        }

        internal static IMyCouchStore CreateStore()
        {
            return new MyCouchStore(CreateUri());
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
