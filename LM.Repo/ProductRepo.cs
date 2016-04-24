using LM.Component.Data;
using LM.Component.Data.Repo;
using LM.Model.Entity;

namespace LM.Repo
{
    public class ProductRepo : BaseRepo<Product>
    {
        public ProductRepo(DbSession dbSession) : base(dbSession) { }
    }
}
