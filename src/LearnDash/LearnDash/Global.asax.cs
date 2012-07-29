using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Core.Logging;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using LearnDash.Dal.NHibernate;
using LearnDash.Windsor;
using LearnDash.Windsor.Installers;
using LearnDash.Services;
using Core.Extensions;

namespace LearnDash
{
    public class MvcApplication : HttpApplication
    {
        private static IWindsorContainer container;

        public static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

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
                defaults: new { id = RouteParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            BootstrapContainer();

            BundleTable.Bundles.RegisterTemplateBundles();

            var bundle = new Bundle("~/Scripts/customjs");
            bundle.AddFile("~/Scripts/jquery.event.drag-2.0.min.js");
            bundle.AddFile("~/Scripts/jquery.event.drop-2.0.min.js");
            bundle.AddFile("~/Scripts/learningflow.js");
            bundle.AddFile("~/Scripts/userecho.js");
            BundleTable.Bundles.Add(bundle);

            bundle = new Bundle("~/Content/twitterbootstrap/bootstrapcss");
            bundle.AddFile("~/Content/twitterbootstrap/css/bootstrap.css");
            bundle.AddFile("~/Content/twitterbootstrap/css/bootstrap-responsive.css");
            BundleTable.Bundles.Add(bundle);

            bundle = new Bundle("~/Content/twitterbootstrap/bootstrapjs");
            bundle.AddFile("~/Content/twitterbootstrap/js/bootstrap.js");
            BundleTable.Bundles.Add(bundle);

            Logger.Info("Application start finished");
        }

        private static void BootstrapContainer()
        {
            container = new WindsorContainer();

            // ISessionFactoryProvider doesnt have implemention by using TypedFactoryFacility castle will provide its own default factory
            container.Kernel.AddFacility<TypedFactoryFacility>();

            // installing all the castle providers
            container.Install(
                new NHibernateInstaller(),
                new RepositoryInstaller(),
                new ControllersInstaller(),
                new ServicesInstaller());

            // binding mvc controller factory with new factory that uses windsor
            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            // add container to session manager, container is then used to resolve some services
            SessionManager.Container = container;
        }

        protected void Application_End()
        {
            container.Dispose();
        }

        public void Application_Error(object sender, EventArgs e)
        {
            var ctx = HttpContext.Current;

            var exception = ctx.Server.GetLastError();

            Logger.ErrorExceptionsWithInner("Unhandled Application Error", exception);
            Logger.Error("Requested url : {0} StackTrace : \r\n{1}", Request.RawUrl, exception.StackTrace);
        }
    }
}