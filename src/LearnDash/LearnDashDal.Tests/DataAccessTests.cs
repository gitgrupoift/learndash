using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LearnDash.Dal.NHibernate;
using NUnit.Framework;

namespace LearnDashDal.Tests
{
    [TestFixture]
    class DataAccesTests
    {
        [Test]
        public void Can_create_access_to_db_in_memory()
        {
            new SqlLiteTestDBAccess();
            using (var session = DataAccess.OpenSession())
            {
                Assert.That(session.Connection, Is.InstanceOf(typeof(System.Data.SQLite.SQLiteConnection)));
            }
        }

        [Test]
        public void By_default_creates_access_to_db_in_sql()
        {
            using (var session = DataAccess.OpenSession())
            {
                Assert.That(session.Connection, Is.InstanceOf(typeof(System.Data.SqlClient.SqlConnection)));
            }
        }

    }
}
