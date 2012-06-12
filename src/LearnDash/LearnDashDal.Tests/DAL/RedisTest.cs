using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LearnDash.Dal;
using NUnit.Framework;

namespace LearnDashDal.Tests.DAL
{
    [TestFixture]
    class RedisTest
    {
        [Test]
        public void Check_if_external_redis_online()
        {
            var password = "b4e641f05d9160d2e508e21d41359d4e";
            var host = "fish.redistogo.com:9261";
            RedisServer server = new RedisServer(password, host);
            
            var result = server.Ping();

            Assert.IsTrue(result);
        }
    }
}
