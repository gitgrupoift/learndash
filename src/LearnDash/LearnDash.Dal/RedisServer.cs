using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;

namespace LearnDash.Dal
{
    public class RedisServer
    {
        private string _password ;
        private string _host;

        public RedisServer(string password, string host)
        {
            _password = password;
            _host = host;
        }

        public bool Ping()
        {
            var host = _host.Split(':');
            var nativeClient = new RedisNativeClient(host[0],Int32.Parse(host[1]));
            nativeClient.Password = _password;
            return nativeClient.Ping();
        }
    }
}
