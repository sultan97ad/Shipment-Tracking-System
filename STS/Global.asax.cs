using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Threading;
using System.Globalization;
using System.Web.Http;
using STS.Controllers;

namespace STS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest(object Sender, EventArgs e)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["language"];
            String Culture = null;
            if (cookie != null)
            {
                switch (cookie.Value)
                {
                    case "Ar":
                        Culture = "ar-SA";
                        break;
                    case "En":
                        Culture = "en-US";
                        break;
                    default:
                        Culture = "en-US";
                        break;
                }
            }
            else
            {
                Culture = "en-US";
            }
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Culture);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Culture);
        }
    }
}
