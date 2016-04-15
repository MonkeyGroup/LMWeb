using System.Collections.Generic;

namespace LM.Component.Data.Repo
{
    public interface IRepository<T> where T : class
    {
        int Insert(T entity);

        bool Insert(IEnumerable<T> entities);

        bool Update(object entity);

        bool Delete(T entity);

        bool Delete(IEnumerable<int> ids);

        T Get(long id);
    }

}
