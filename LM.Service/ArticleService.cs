using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LM.Component.Data;
using LM.Model.Entity;
using LM.Repo;
using LM.Service.Base;

namespace LM.Service
{
    public class ArticleService : BaseService
    {
        private readonly ArticleRepo _articleRepo;

        public ArticleService(DbSession dbSession)
            : base(dbSession)
        {
            _articleRepo = new ArticleRepo(dbSession);
        }


        public ServiceResult Insert(Article article)
        {
            try
            {
                article.Id = _articleRepo.Insert(article);
                return article.Id > 0 ? new ServiceResult(true, ServiceResultCode.正常, "成功") : new ServiceResult(false, ServiceResultCode.服务器异常, ServiceResultCode.服务器异常.ToString());
            }
            catch (Exception e)
            {
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
        }

    }
}
