﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;

namespace LearnDash
{
    public interface ISessionFactoryProvider
    {
        IEnumerable<ISessionFactory> GetSessionFactories();
    }
}