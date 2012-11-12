using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using LearnDash.Dal.NHibernate;
using NLog;

namespace LearnDash.Windsor.Installers
{
    public class RepositoryInstaller : IWindsorInstaller
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            logger.Info("Installing repository in container");

            container.Register(Component.For(typeof(IRepository<>)).ImplementedBy(typeof(Repository<>)));

            logger.Info("Repository installed in container");
        }
    }
}