using LM.Component.Data;
using LM.Component.Data.Repo;
using LM.Model.Entity;

namespace LM.Repo
{
    public class BaseInfoRepo : BaseRepo<BaseInfo>
    {
        public BaseInfoRepo(DbSession dbSession) : base(dbSession) { }

    }
}
