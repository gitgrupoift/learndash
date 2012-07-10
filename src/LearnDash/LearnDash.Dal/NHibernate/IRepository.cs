using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnDash.Dal.NHibernate
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        int? Add(T item);
        void Remove(T item);
        bool Update(T item);
        int GetCount();
        IList<T> GetAll();
        IList<T> GetByParameterEqualsFilter(string parameterName, object value);
        IList<T> GetByQuery(string query);
        IList<T> GetByQueryObject(IQueryObject query);
    }
}
