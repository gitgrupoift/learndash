using System.Collections.Generic;
using NHibernate;

namespace LearnDash
{
    public interface ISessionFactoryProvider
    {
        IEnumerable<ISessionFactory> GetSessionFactories();
    }
}