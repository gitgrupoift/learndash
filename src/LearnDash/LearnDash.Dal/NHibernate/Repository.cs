using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using Core.Extensions;

namespace LearnDash.Dal.NHibernate
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        protected readonly ISessionFactory Factory;

        public Repository(ISessionFactory factory)
        {
            Factory = factory;
        }

        protected ISession Session
        {
            get { return Factory.GetCurrentSession(); }
        }

        public T GetById(int id)
        {
            try
            {
                var obj = Session.Get<T>(id);
                return obj;
            }
            catch (Exception ex)
            {
                Logger.ErrorExceptionsWithInner("GetById Method", ex);
                return null;
            }
        }

        public int? Add(T item)
        {
            int? addedItemId;
            using (var transaction = Session.BeginTransaction())
            {
                try
                {
                    addedItemId = (int)Session.Save(item);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Logger.ErrorExceptionsWithInner("Add Method", ex);
                    transaction.Rollback();
                    addedItemId = null;
                }
                finally
                {
                    transaction.Dispose();
                    
                }
            }
            return addedItemId;
        }

        public bool Remove(T item)
        {
            using (var transaction = Session.BeginTransaction())
            {
                var result = true;
                try
                {
                    Session.Delete(item);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Logger.ErrorExceptionsWithInner("Remove Method", ex);
                    transaction.Rollback();
                    result = false;
                }
                finally
                {
                    transaction.Dispose();
                }
                return result;
            }
        }

        public bool Update(T item)
        {
            using (var transaction = Session.BeginTransaction())
            {
                try
                {
                    Session.SaveOrUpdate(item);
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.ErrorExceptionsWithInner("Update Method", ex);
                    transaction.Rollback();
                }
                finally
                {
                    transaction.Dispose();
                }
            }
            return false;
        }

        public int GetCount()
        {
            try
            {
                var count = Session.CreateCriteria(typeof(T)).List<T>().Count;
                return count;
            }
            catch (Exception ex)
            {
                Logger.ErrorExceptionsWithInner("GetCount Method", ex);
                return 0;
            }
        }
        public IList<T> GetAll()
        {
            try
            {
                var returnedList = Session.CreateCriteria(typeof(T)).SetCacheable(true).List<T>();
                return returnedList;
            }
            catch (Exception ex)
            {
                Logger.ErrorExceptionsWithInner("GetAll Method", ex);
                return new List<T>();
            }
        }

        public IList<T> GetByParameterEqualsFilter(string parameterName, object value)
        {
            try
            {
                var returnedList = Session.CreateCriteria(typeof(T)).Add(Restrictions.Eq(parameterName, value)).List<T>();
                return returnedList;
            }
            catch (Exception ex)
            {
                Logger.ErrorExceptionsWithInner("GetByParameterEqualsFilter Method", ex);
                return new List<T>();
            }
        }

        public IList<T> GetByQuery(string query)
        {
            try
            {
                var returnedList = Session.CreateQuery(query).List<T>();
                return returnedList;
            }
            catch (Exception ex)
            {
                Logger.ErrorExceptionsWithInner("GetByQuery Method", ex);
                return new List<T>();
            }
        }

        public IList<T> GetByQueryObject(IQueryObject query)
        {
            try
            {
                return GetByQuery(query.Query);

            }
            catch (Exception ex)
            {
                Logger.ErrorExceptionsWithInner("GetByQueryObject Method", ex);
                return new List<T>();
            }
        }
    }
}
