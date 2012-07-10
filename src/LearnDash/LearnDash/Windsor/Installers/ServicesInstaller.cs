using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using LearnDash.Services;

namespace LearnDash.Windsor.Installers
{
    public class ServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromThisAssembly()
                                .Where(Component.IsInSameNamespaceAs(typeof(ILearningFlowService)))
                                .WithService.DefaultInterfaces()
                                .Configure(c => c.LifestylePerWebRequest()));
        }
    }
}