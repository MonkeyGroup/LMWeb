using LM.Component.Data;
using LM.Component.Data.Repo;
using LM.Model.Entity;

namespace LM.Repo.Repo
{
    public class UserRepo : BaseRepo<User>
    {
        public UserRepo(DbSession dbSession) : base(dbSession) { }
        
    }
}
