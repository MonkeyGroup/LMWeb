using LM.Component.Data;
using LM.Component.Data.Repo;
using LM.Model.Entity;

namespace LM.Repo
{
    public class CategoryRepo : BaseRepo<Category>
    {
        public CategoryRepo(DbSession dbSession) : base(dbSession) { }

    }
}
