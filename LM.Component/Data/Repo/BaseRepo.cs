using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace LM.Component.Data.Repo
{
    /// <summary>
    ///  基于 Dapper 的单表CRUD仓储库
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseRepo<T> : IRepository<T> where T : class
    {
        private DbSession DbSession { get; set; }

        protected BaseRepo(DbSession dbSession)
        {
            DbSession = dbSession;
        }


        #region CRUD methods for this table. 
        public int Insert(T entity)
        {
            return DbSession.Connection.Insert<T>(entity);
        }

        public bool Insert(IEnumerable<T> entities)
        {
            return DbSession.Connection.Insert<T>(entities);
        }

        public bool Update(object entity)
        {
            return DbSession.Connection.Update<T>(entity);
        }

        public bool Delete(T entity)
        {
            return DbSession.Connection.Delete<T>(entity);
        }

        public bool Delete(IEnumerable<int> ids)
        {
            return DbSession.Connection.Delete<T>(ids);
        }

        public T Get(long id)
        {
            return DbSession.Connection.Get<T>(id);
        }

        #endregion
        
    }
}
