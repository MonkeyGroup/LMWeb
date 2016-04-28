using LM.Component.Data;
using LM.Component.Data.Repo;
using LM.Model.Entity;

namespace LM.Repo
{
    public class BriefRepo : BaseRepo<Brief>
    {
        public BriefRepo(DbSession dbSession) : base(dbSession) { }

    }
}
