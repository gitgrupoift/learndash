using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LearnDash.Dal.NHibernate;
using NUnit.Framework;

namespace LearnDashDal.Tests
{
    [TestFixture]
    class NHibernateTest
    {
        [Test]
        public void Reset_schema()
        {
            DataAccess.ResetDb();
        }
    }
}
