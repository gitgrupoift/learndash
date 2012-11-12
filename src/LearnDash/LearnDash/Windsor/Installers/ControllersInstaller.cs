namespace LearnDash.Windsor.Installers
{
    using System;
    using System.Web.Mvc;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Controllers;
    using NLog;

    public class ControllersInstaller : IWindsorInstaller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            try
            {
                Logger.Info("Installing Controllers in container");

                container.Register(
                    AllTypes.FromThisAssembly().BasedOn<IController>().If(
                        Component.IsInSameNamespaceAs<HomeController>()).If(t => t.Name.EndsWith("Controller")).
                        Configure(c => c.LifestyleTransient()));

                Logger.Info("Controllers isntalled in container");

            }
            catch(Exception ex)
            {
                Logger.Fatal("Encountered error - {0}", ex.Message);
            }
        }
    }
}