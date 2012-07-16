namespace LearnDash.Windsor.Installers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using LearnDash.Controllers;
    using NLog;

    public class ControllersInstaller : IWindsorInstaller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            try
            {

                container.Register(
                    AllTypes.FromThisAssembly().BasedOn<IController>().If(
                        Component.IsInSameNamespaceAs<HomeController>()).If(t => t.Name.EndsWith("Controller")).
                        Configure(c => c.LifestyleTransient()));

            }
            catch(Exception ex)
            {
                Logger.Fatal("Encountered error - {0}", ex.Message);
            }
        }
    }
}