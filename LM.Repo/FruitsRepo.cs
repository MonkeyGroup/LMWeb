using LM.Component.Data;
using LM.Component.Data.Repo;
using LM.Model.Entity;

namespace LM.Repo
{
    public class FruitsRepo : BaseRepo<Fruits>
    {
        public FruitsRepo(DbSession dbSession) : base(dbSession) { }

    }
}
