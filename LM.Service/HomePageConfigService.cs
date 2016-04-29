using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LM.Component.Data;
using LM.Model.Entity;
using LM.Model.Model;
using LM.Repo;
using LM.Service.Base;

namespace LM.Service
{
    public class HomePageConfigService : BaseService
    {
        private readonly HomePageConfigRepo _homePageConfigRepo;

        public HomePageConfigService(DbSession dbSession)
            : base(dbSession)
        {
            _homePageConfigRepo = new HomePageConfigRepo(dbSession);
        }


        #region CRUD操作

        public ServiceResult Insert(HomePageConfig config)
        {
            try
            {
                config.Id = _homePageConfigRepo.Insert(config);
                return config.Id > 0 ? new ServiceResult(true, ServiceResultCode.正常, "成功") : new ServiceResult(false, ServiceResultCode.服务器异常, ServiceResultCode.服务器异常.ToString());
            }
            catch (Exception e)
            {
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
        }

        public ServiceResult GetById(long id)
        {
            try
            {
                var config = _homePageConfigRepo.Get(id);
                return new ServiceResult(true, ServiceResultCode.正常, "成功", config);
            }
            catch (Exception e)
            {
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
        }

        /// <summary>
        ///  获取最新的一条配置信息
        /// </summary>
        /// <returns></returns>
        public ServiceResult GetLast()
        {
            try
            {
                var configs = QueryManage.GetList<HomePageConfigModel>("select * from [HomePageConfig] where id >100 order by [Id] desc;");
                return configs.Count > 0 ? new ServiceResult(true) { Data = configs.FirstOrDefault() } : new ServiceResult(false) { Message = "无数据" };
            }
            catch (Exception e)
            {
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
        }

        #endregion


    }
}
