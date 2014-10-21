using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Samples.Mvc5WithIdentity2.Startup))]
namespace Samples.Mvc5WithIdentity2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
