using LM.Component.Data;
using LM.Component.Data.Repo;
using LM.Model.Entity;

namespace LM.Repo
{
    public class CompanyRepo : BaseRepo<Company>
    {
        public CompanyRepo(DbSession dbSession) : base(dbSession) { }
    }
}
