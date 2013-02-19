namespace LearnDash.Windsor.Installers
{
    using System;
    using System.Web.Http;
    using System.Web.Mvc;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Controllers;
    using NLog;

    public class ApiControllersInstaller : IWindsorInstaller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            try
            {
                logger.Info("Installing Api Controllers in container");

                container.Register(
                    AllTypes.FromThisAssembly()
                            .BasedOn<ApiController>()
                            .If(Component.IsInSameNamespaceAs<FlowsController>())
                            .If(t => t.Name.EndsWith("Controller"))
                            .Configure(c => c.LifestyleTransient()));

                logger.Info("Controllers Api  installed in container");
            }
            catch (Exception ex)
            {
                logger.Fatal("Encountered error - {0}", ex.Message);
            }
        }
    }
}