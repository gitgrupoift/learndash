﻿namespace LearnDash.Windsor.Installers
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
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            try
            {
                logger.Info("Installing Controllers in container");

                container.Register(
                    AllTypes.FromThisAssembly()
                            .BasedOn<IController>()
                            .If(Component.IsInSameNamespaceAs<HomeController>())
                            .If(t => t.Name.EndsWith("Controller"))
                            .Configure(c => c.LifestyleTransient()));

                logger.Info("Controllers isntalled in container");
            }
            catch (Exception ex)
            {
                logger.Fatal("Encountered error - {0}", ex.Message);
            }
        }
    }
}