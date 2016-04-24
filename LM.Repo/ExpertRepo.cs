using LM.Component.Data;
using LM.Component.Data.Repo;
using LM.Model.Entity;

namespace LM.Repo
{
    public class ExpertRepo : BaseRepo<Expert>
    {
        public ExpertRepo(DbSession dbSession) : base(dbSession) { }
    }
}
