using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LearnDash
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            BundleTable.Bundles.RegisterTemplateBundles();

            var bundle = new Bundle("~/Scripts/customjs");
            bundle.AddFile("~/Scripts/jquery.roundabout.js");
            bundle.AddFile("~/Scripts/jquery.event.drag-2.0.min.js");
            bundle.AddFile("~/Scripts/jquery.event.drop-2.0.min.js");
            bundle.AddFile("~/Scripts/jquery.roundabout-shapes.min.js");
            BundleTable.Bundles.Add(bundle);

            bundle = new Bundle("~/Content/twitterbootstrap/bootstrapcss");
            bundle.AddFile("~/Content/twitterbootstrap/css/bootstrap.css");
            bundle.AddFile("~/Content/twitterbootstrap/css/bootstrap-responsive.css");
            BundleTable.Bundles.Add(bundle);

            bundle = new Bundle("~/Content/twitterbootstrap/bootstrapjs");
            bundle.AddFile("~/Content/twitterbootstrap/js/bootstrap.js");

            BundleTable.Bundles.Add(bundle);
        }
    }
}