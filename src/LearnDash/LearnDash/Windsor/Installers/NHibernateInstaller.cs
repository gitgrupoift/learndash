namespace LearnDash.Windsor.Installers
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Configuration;
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using Dal.NHibernate;
    using Dal.NHibernate.Mappings;
    using NHibernate;
    using NLog;

    using Environment = NHibernate.Cfg.Environment;

    public class NHibernateInstaller : IWindsorInstaller
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        private const string TestConnString = "Data Source=.\\SQLEXPRESS;Initial Catalog=LearnDash;Integrated Security=SSPI";
        private  string ConnectionString
        {
            get
            {
                if (WebConfigurationManager.ConnectionStrings["DBConString"] != null)
                {
                    return WebConfigurationManager.ConnectionStrings["DBConString"].ConnectionString;
                }
                return TestConnString;
            }
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            logger.Info("Installing NHibernate in container");

            container.Register(Component.For<ISessionFactory>()
                                   .UsingFactoryMethod(k => BuildSessionFactory()));

            container.Register(Component.For<NHibernateSessionModule>());

            container.Register(Component.For<ISessionFactoryProvider>().AsFactory());

            container.Register(Component.For<IEnumerable<ISessionFactory>>()
                                        .UsingFactoryMethod(k => k.ResolveAll<ISessionFactory>()));

            HttpContext.Current.Application[SessionFactoryProvider.Key] = container.Resolve<ISessionFactoryProvider>();

            logger.Info("NHibernate installed in container");
        }

        private ISessionFactory BuildSessionFactory()
        {
            try
            {
                var configuration =
                    Fluently.Configure().Database(MsSqlConfiguration.MsSql2008.ConnectionString(ConnectionString)).
                        Mappings(x => x.FluentMappings.AddFromAssemblyOf<UserProfileMap>()).ExposeConfiguration(
                            c =>
                                {
                                    // People advice not to use NHibernate.Cache.HashtableCacheProvider for production
                                    c.SetProperty("cache.provider_class", "NHibernate.Cache.HashtableCacheProvider");
                                    c.SetProperty("cache.use_second_level_cache", "true");
                                    c.SetProperty("cache.use_query_cache", "true");
                                    c.SetProperty(
                                        Environment.CurrentSessionContextClass,
                                        typeof(LazySessionContext).AssemblyQualifiedName);
                                });
                return configuration.BuildConfiguration().BuildSessionFactory();
            }
            catch (Exception e)
            {
                this.logger.ErrorException("Encountered error while configuring database", e);
                throw new DatabaseConnectionException();
            }
        }
    }

    internal class DatabaseConnectionException : Exception
    {
    }
}