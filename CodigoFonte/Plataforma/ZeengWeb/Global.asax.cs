using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ZRN.Usuarios;

namespace ZeengWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
        }


        public void Application_AcquireRequestState(object sender, EventArgs e)
        {

            if (System.Web.HttpContext.Current.Session == null)
                return;

            if (HttpContext.Current.Request.Url.LocalPath != "/Login/Index" && HttpContext.Current.Request.Url.LocalPath != "/login/login")
            {
                if (System.Web.HttpContext.Current.Session["usuario"] == null)
                {
                    Response.Redirect("/Login/Index");
                }
            }

        }

        public void Session_Start(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("/Login/Index");
            }
        }

    }
}
