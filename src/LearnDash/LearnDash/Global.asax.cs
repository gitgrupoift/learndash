namespace LearnDash
{
    using System;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Dispatcher;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    using Castle.Facilities.TypedFactory;
    using Castle.Windsor;

    using Core.Extensions;

    using LearnDash.Windsor;
    using LearnDash.Windsor.Installers;

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
                defaults: new { id = UrlParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }

        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
                
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
            bundle.AddFile("~/Scripts/layout.js");
            BundleTable.Bundles.Add(bundle);

            bundle = new Bundle("~/Content/customcss");
            bundle.AddFile("~/Content/themes/black-tie/jquery-ui-1.9.1.custom.css");
            BundleTable.Bundles.Add(bundle);

            bundle = new Bundle("~/Content/twitterbootstrap/bootstrapcss");
            bundle.AddFile("~/Content/twitterbootstrap/css/bootstrap.css");
            bundle.AddFile("~/Content/twitterbootstrap/css/bootstrap-responsive.css");
            BundleTable.Bundles.Add(bundle);

            bundle = new Bundle("~/Content/twitterbootstrap/bootstrapjs");
            bundle.AddFile("~/Content/twitterbootstrap/js/bootstrap.js");
            BundleTable.Bundles.Add(bundle);

            bundle = new Bundle("~/Content/notyjs");
            bundle.AddFile("~/Content/noty/jquery.noty.js");
            bundle.AddFile("~/Content/noty/layouts/top.js");
            bundle.AddFile("~/Content/noty/layouts/inline.js");
            bundle.AddFile("~/Content/noty/layouts/topcenter.js");
            bundle.AddFile("~/Content/noty/themes/default.js");
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
                new ServicesInstaller(),
                new ApiControllersInstaller());

            // binding mvc controller factory with new factory that uses windsor
            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            // add container to session manager, container is then used to resolve some services
            SessionManager.Container = container;

            GlobalConfiguration.Configuration.Services.Replace(
                typeof(IHttpControllerActivator),
                new WindsorCompositionRoot(container));
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
            Logger.Error(string.Format("Request Url : {0}", ctx.Request.Url));
        }
    }
}