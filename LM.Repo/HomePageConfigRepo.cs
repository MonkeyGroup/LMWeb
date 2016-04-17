using LM.Component.Data;
using LM.Component.Data.Repo;
using LM.Model.Entity;

namespace LM.Repo
{
    public class HomePageConfigRepo : BaseRepo<HomePageConfig>
    {
        public HomePageConfigRepo(DbSession dbSession) : base(dbSession) { }

    }
}
