using System.Collections.Generic;
using System.Linq.Expressions;

namespace LM.Component.Data.Repo
{
    public interface IRepository<T> where T : class
    {
        long Insert(T entity);
        
        void Insert(IEnumerable<T> entities);

        void Update(T entity);

        void UpdatePart(object entity);
        
        void Delete(long id);

        void Delete(IEnumerable<long> id);

        T Get(long id);
    }

}
