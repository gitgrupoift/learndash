using System;
using System.Web.Configuration;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace LearnDash.Dal.NHibernate
{
    public static class DataAccess
    {
        public const string TestConnString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=LearnDash;Integrated Security=SSPI";

        private static string ConnectionString
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
        private static ISessionFactory _sessionFactory;
        private static readonly object SyncRoot = new object();
        private static Configuration _configuration;

        private static readonly Func<ISession> DefaultOpenSession = () =>
        {
            if (_sessionFactory == null)
            {
                lock (SyncRoot)
                {
                    if (_sessionFactory == null)
                        Configure();
                }

            }

            return _sessionFactory.OpenSession();
        };

        private static void Configure()
        {
            _sessionFactory = Configuration.BuildSessionFactory();
        }

        public static Configuration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    _configuration = Fluently.Configure().
                            Database(MsSqlConfiguration.MsSql2008.ConnectionString(ConnectionString))
                            .Mappings(x => x.FluentMappings.AddFromAssembly(System.Reflection.Assembly.GetExecutingAssembly()))
                            .ExposeConfiguration(c =>
                            {
                                // People advice not to use NHibernate.Cache.HashtableCacheProvider for production
                                c.SetProperty("cache.provider_class", "NHibernate.Cache.HashtableCacheProvider");
                                c.SetProperty("cache.use_second_level_cache", "true");
                                c.SetProperty("cache.use_query_cache", "true");
                            })
                             .BuildConfiguration();
                }
                return _configuration;
            }
            set
            {
                _configuration = value;
            }
        }


        [ThreadStatic]
        private static Func<ISession> _openSession;

        public static Func<ISession> OpenSession { get; set; }

        public static void InTransaction(Action<ISession> operation)
        {
            using (var session = OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    operation(session);

                    tx.Commit();
                }
            }
        }

        public static void ResetDb()
        {
            new SchemaUpdate(Configuration).Execute(true, true);
        }
    }
}
