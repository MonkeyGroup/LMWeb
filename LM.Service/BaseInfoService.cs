using System;
using LM.Component.Data;
using LM.Model.Entity;
using LM.Repo;
using LM.Service.Base;

namespace LM.Service
{
    public class BaseInfoService : BaseService
    {
        private readonly BaseInfoRepo _baseInfoRepo;

        public BaseInfoService(DbSession dbSession)
            : base(dbSession)
        {
            _baseInfoRepo = new BaseInfoRepo(dbSession);
        }


        #region CRUD

        public ServiceResult GetById(long id)
        {
            try
            {
                var novel = _baseInfoRepo.Get(id);
                return new ServiceResult(true, ServiceResultCode.正常, "成功", novel);
            }
            catch (Exception e)
            {
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
        }

        public ServiceResult Insert(BaseInfo baseInfo)
        {
            try
            {
                baseInfo.Id = _baseInfoRepo.Insert(baseInfo);
                return baseInfo.Id > 0 ?
                    new ServiceResult(true, ServiceResultCode.正常, "成功") :
                    new ServiceResult(false, ServiceResultCode.服务器异常, ServiceResultCode.服务器异常.ToString());
            }
            catch (Exception e)
            {
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
        }

        public ServiceResult Update(object baseInfo)
        {
            try
            {
                var state = _baseInfoRepo.Update(baseInfo);
                return state ?
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
