namespace LearnDash.Windsor.Installers
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    using LearnDash.Services;

    using NLog;

    public class ServicesInstaller : IWindsorInstaller
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            logger.Info("Installing services in container");

            container.Register(AllTypes.FromThisAssembly()
                                .Where(Component.IsInSameNamespaceAs(typeof(ILearningFlowService)))
                                .WithService.DefaultInterfaces()
                                .Configure(c => c.LifestylePerWebRequest()));

            logger.Info("Service in container installed");
        }
    }
}