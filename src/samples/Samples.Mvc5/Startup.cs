using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Samples.Mvc5.Startup))]
namespace Samples.Mvc5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
