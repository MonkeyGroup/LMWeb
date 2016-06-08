using LM.Component.Data;
using LM.Component.Data.Repo;
using LM.Model.Entity;

namespace LM.Repo
{
    public class AdRepo : BaseRepo<Ad>
    {
        public AdRepo(DbSession dbSession) : base(dbSession) { }
    }
}
