using System;
using System.Linq;
using LM.Component.Data;
using LM.Model.Entity;
using LM.Model.Model;
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
                var baseInfo = _baseInfoRepo.Get(id);
                return new ServiceResult(true, ServiceResultCode.正常, "成功", baseInfo);
            }
            catch (Exception e)
            {
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
        }

        /// <summary>
        ///  获取最新的一条机构信息
        /// </summary>
        /// <returns></returns>
        public ServiceResult GetLast()
        {
            try
            {
                var baseInfos = QueryManage.GetList<BaseInfoModel>("select * from [BaseInfo] order by [Id] desc;");
                return baseInfos.Count > 0 ? new ServiceResult(true) { Data = baseInfos.FirstOrDefault() } : new ServiceResult(false) { Message = "无数据" };
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
