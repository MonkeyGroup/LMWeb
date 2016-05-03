using System;
using System.Linq;
using LM.Component.Data;
using LM.Model.Entity;
using LM.Model.Model;
using LM.Repo;
using LM.Service.Base;

namespace LM.Service
{
    public class CompanyService : BaseService
    {
        private readonly CompanyRepo _companyRepo;

        public CompanyService(DbSession dbSession)
            : base(dbSession)
        {
            _companyRepo = new CompanyRepo(dbSession);
        }


        #region CRUD 方法

        public ServiceResult GetById(long id)
        {
            try
            {
                var company = _companyRepo.Get(id);
                return new ServiceResult(true, ServiceResultCode.正常, "成功", company);
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
                var models = QueryManage.GetList<CompanyModel>(query, param);
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
                var companies = QueryManage.GetListByPage<CompanyModel>(targetQuery, orderby, out  itemCount, pageIndex, pageSize).ToList();
                return new ServiceResult(true) { Data = companies };
            }
            catch (Exception e)
            {
                itemCount = 0;
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
        }

        public ServiceResult Insert(Company company)
        {
            try
            {
                return  _companyRepo.Insert(company) > 0 ? 
                    new ServiceResult(true, ServiceResultCode.正常, "成功") : 
                    new ServiceResult(false, ServiceResultCode.服务器异常, ServiceResultCode.服务器异常.ToString());
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
                return _companyRepo.Update(article) ? 
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
                return _companyRepo.Delete(new Company { Id = id }) ? 
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
                return _companyRepo.Delete(ids.Distinct()) ? 
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
