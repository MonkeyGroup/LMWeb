using LM.Component.Data;
using LM.Component.Data.Repo;
using LM.Model.Entity;

namespace LM.Repo
{
    public class ArticleRepo : BaseRepo<Article>
    {
        public ArticleRepo(DbSession dbSession) : base(dbSession) { }
        
    }
}
