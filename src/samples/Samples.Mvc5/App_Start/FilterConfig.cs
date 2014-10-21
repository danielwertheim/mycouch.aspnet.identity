using System.Web;
using System.Web.Mvc;

namespace Samples.Mvc5WithIdentity2
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
