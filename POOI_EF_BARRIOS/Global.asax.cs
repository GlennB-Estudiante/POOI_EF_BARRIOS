using System.Web.Mvc;
using System.Web.Routing;

namespace POOI_EF
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteTable.Routes.MapRoute("Default", "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}
