using System;
using System.Linq;
using LM.Component.Data;
using LM.Model.Entity;
using LM.Model.Model;
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

        #region CRUD 方法
        public ServiceResult GetById(long id)
        {
            try
            {
                var article = _articleRepo.Get(id);
                return new ServiceResult(true, ServiceResultCode.正常, "成功", article);
            }
            catch (Exception e)
            {
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
        }

        /// <summary>
        ///  此方法废弃，因为不支持 out 参数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ServiceResult GetByPage(int pageIndex, int pageSize)
        {
            try
            {
                const string query = "[Article]";
                int pageCount;
                var articles = QueryManage.GetListByPage<ArticleModel>(query, "createat desc", out  pageCount, pageIndex, pageSize).ToList();
                return new ServiceResult(true) { Data = articles };
            }
            catch (Exception e)
            {
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
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

        public ServiceResult Update(object article)
        {
            try
            {
                var state = _articleRepo.Update(article);
                return state ? new ServiceResult(true, ServiceResultCode.正常, "成功") : new ServiceResult(false, ServiceResultCode.服务器异常, ServiceResultCode.服务器异常.ToString());
            }
            catch (Exception e)
            {
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
        }

        public ServiceResult Delete(int id)
        {
            try
            {
                var state = _articleRepo.Delete(new Article { Id = id });
                return state ? new ServiceResult(true, ServiceResultCode.正常, "成功") : new ServiceResult(false, ServiceResultCode.服务器异常, ServiceResultCode.服务器异常.ToString());
            }
            catch (Exception e)
            {
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
        }

        public ServiceResult Delete(int[] ids)
        {
            try
            {
                var state = _articleRepo.Delete(ids.Distinct());
                return state ? new ServiceResult(true, ServiceResultCode.正常, "成功") : new ServiceResult(false, ServiceResultCode.服务器异常, ServiceResultCode.服务器异常.ToString());
            }
            catch (Exception e)
            {
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
        }

        #endregion

    }
}
