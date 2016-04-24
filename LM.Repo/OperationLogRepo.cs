using LM.Component.Data;
using LM.Component.Data.Repo;
using LM.Model.Entity;

namespace LM.Repo
{
    public class OperationLogRepo : BaseRepo<OperationLog>
    {
        public OperationLogRepo(DbSession dbSession)
            : base(dbSession)
        {
        }
    }
}
