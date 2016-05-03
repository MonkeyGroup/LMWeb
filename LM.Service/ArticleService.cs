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
                var entity = _articleRepo.Get(id);
                return new ServiceResult(true, ServiceResultCode.正常, "成功", entity);
            }
            catch (Exception e)
            {
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
        }

        public ServiceResult GetList(string query, object param = null)
        {
            try
            {
                var models = QueryManage.GetList<ArticleModel>(query, param);
                return new ServiceResult(true, ServiceResultCode.正常, "成功", models);
            }
            catch (Exception e)
            {
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
        }

        public ServiceResult GetByPage(string targetQuery, string orderby, int pageIndex, int pageSize, out int itemCount)
        {
            try
            {
                var entities = QueryManage.GetListByPage<ArticleModel>(targetQuery, orderby, out  itemCount, pageIndex, pageSize).ToList();
                return new ServiceResult(true) { Data = entities };
            }
            catch (Exception e)
            {
                itemCount = 0;
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
        }

        public ServiceResult Insert(Article entity)
        {
            try
            {
                return _articleRepo.Insert(entity) > 0 ?
                    new ServiceResult(true, ServiceResultCode.正常, "成功") :
                    new ServiceResult(false, ServiceResultCode.服务器异常, ServiceResultCode.服务器异常.ToString());
            }
            catch (Exception e)
            {
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
        }

        public ServiceResult Update(object model)
        {
            try
            {
                return _articleRepo.Update(model) ?
                    new ServiceResult(true, ServiceResultCode.正常, "成功") :
                    new ServiceResult(false, ServiceResultCode.服务器异常, ServiceResultCode.服务器异常.ToString());
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
                return _articleRepo.Delete(new Article { Id = id }) ?
                    new ServiceResult(true, ServiceResultCode.正常, "成功") :
                    new ServiceResult(false, ServiceResultCode.服务器异常, ServiceResultCode.服务器异常.ToString());
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
                return _articleRepo.Delete(ids.Distinct()) ?
                    new ServiceResult(true, ServiceResultCode.正常, "成功") :
                    new ServiceResult(false, ServiceResultCode.服务器异常, ServiceResultCode.服务器异常.ToString());
            }
            catch (Exception e)
            {
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
        }

        #endregion

    }
}
