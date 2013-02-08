using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.ApplicationServer.Http.Activation;
using ClientLocate.Resources;
using ClientLocate.Models;

namespace ClientLocate
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
           // filters.Add(new HandleErrorAttribute());
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception unhandledException = Server.GetLastError();
            HttpContext.Current.Response.StatusCode = 500;
            HttpContext.Current.Response.ContentType = HttpContext.Current.Request.ContentType;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(unhandledException.ToString());
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.End();
        }
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.IgnoreRoute("Default.htm");

            //routes.MapRoute(
            //    "Default", // Route name
            //    "{controller}/{action}/{id}", // URL with parameters
            //    new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

        }

        protected void Application_Start()
        {

            RouteTable.Routes.MapServiceRoute<SearchResource>("Search");
            RouteTable.Routes.MapServiceRoute<CountResource>("Count");
            ClientLocateDbContext context = new ClientLocateDbContext();


            //AreaRegistration.RegisterAllAreas();

            //RegisterGlobalFilters(GlobalFilters.Filters);
            //RegisterRoutes(RouteTable.Routes);
        }
    }
}