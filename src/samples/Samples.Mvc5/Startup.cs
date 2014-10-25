using Microsoft.Owin;
using Owin;
using Samples.Mvc5;

[assembly: OwinStartup(typeof(Startup))]
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
