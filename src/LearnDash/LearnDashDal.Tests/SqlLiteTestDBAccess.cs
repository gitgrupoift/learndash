using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using LearnDash.Dal.Models;
using LearnDash.Dal.NHibernate;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace LearnDashDal.Tests
{
    public class SqlLiteTestDBAccess
    {
        private static Configuration _configuration;
        private static ISessionFactory _sessionFactory;
        private readonly ISession _session;

        public SqlLiteTestDBAccess()
        {
            // configure NHibernate once for for all tests
            EnsureConfigured();

            // open one session for each test
            _session = _sessionFactory.OpenSession();

            // recreate the whole test database for each unit test
            RecreateDataBase();

            // replace default session creation logic to return the newly opened session meant to be used throughout the whole test;
            DataAccess.OpenSession = () =>
            {
                Console.WriteLine("----------------------------");
                Console.WriteLine("New session...");
                Console.WriteLine("----------------------------");

                _session.Clear();
                return new UndisposableSession(_session);
            };

        }

        private void RecreateDataBase()
        {
            Console.WriteLine("----------------------------");
            Console.WriteLine("Creating database schema...");
            Console.WriteLine("----------------------------");

            // first param to TRUE if output to console needed; otherwise - FALSE
            new SchemaExport(Configuration).Execute(true, true, false, _session.Connection, null);

            Console.WriteLine("----------------------------");
            Console.WriteLine("Schema created.");
            Console.WriteLine("----------------------------");
        }

        private static readonly object _syncRoot = new object();

        public static Configuration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    _configuration = Fluently.Configure()
                            .Database(() => SQLiteConfiguration.Standard.InMemory().ShowSql())
                            .Mappings(x => x.FluentMappings.AddFromAssembly(typeof(UserProfile).Assembly))
                            .BuildConfiguration();
                }
                return _configuration;
            }
            set
            {
                _configuration = value;
            }
        }

        private void EnsureConfigured()
        {
            _sessionFactory = Configuration.BuildSessionFactory();
        }

        public void Dispose()
        {
            Console.WriteLine("----------------------------");
            Console.WriteLine("Disposing session...");
            Console.WriteLine("----------------------------");
            _session.Dispose();
        }
    }

}
