using System;
using System.Linq;
using LM.Component.Data;
using LM.Model.Entity;
using LM.Model.Model;
using LM.Repo;
using LM.Service.Base;

namespace LM.Service
{
    public class CategoryService : BaseService
    {
        private readonly CategoryRepo _categoryRepo;

        public CategoryService(DbSession dbSession)
            : base(dbSession)
        {
            _categoryRepo = new CategoryRepo(dbSession);
        }


        #region CRUD 方法
        public ServiceResult GetById(long id)
        {
            try
            {
                var entity = _categoryRepo.Get(id);
                return new ServiceResult(true, ServiceResultCode.正常, "成功", entity);
            }
            catch (Exception e)
            {
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
        }

        /// <summary>
        ///  查找分类对象下面的所有分类
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public ServiceResult GetByTarget(string target)
        {
            try
            {
                var query = string.Format(@"select Id,Name from [Category] where [Target] = '{0}' order by [Index] desc, [Name] asc", target);
                var models = QueryManage.GetList<CategoryModel>(query).ToList();
                return new ServiceResult(true) { Data = models };
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
                var entities = QueryManage.GetListByPage<CategoryModel>(targetQuery, orderby, out  itemCount, pageIndex, pageSize).ToList();
                return new ServiceResult(true) { Data = entities };
            }
            catch (Exception e)
            {
                itemCount = 0;
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
        }

        public ServiceResult Insert(Category entity)
        {
            try
            {
                return _categoryRepo.Insert(entity) > 0 ?
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
                return _categoryRepo.Update(model) ?
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
                return _categoryRepo.Delete(new Category { Id = id }) ?
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
                return _categoryRepo.Delete(ids.Distinct()) ?
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
