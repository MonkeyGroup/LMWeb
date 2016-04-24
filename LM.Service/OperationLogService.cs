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
    public class OperationLogService : BaseService
    {
        private readonly OperationLogRepo _operationLogRepo;

        public OperationLogService(DbSession dbSession)
            : base(dbSession)
        {
            _operationLogRepo = new OperationLogRepo(dbSession);
        }


        #region CRUD 方法
        public ServiceResult GetByPage(string targetQuery, string orderby, int pageIndex, int pageSize, out int itemCount)
        {
            try
            {
                var operationLogs = QueryManage.GetListByPage<OperationLogModel>(targetQuery, orderby, out  itemCount, pageIndex, pageSize).ToList();
                return new ServiceResult(true) { Data = operationLogs };
            }
            catch (Exception e)
            {
                itemCount = 0;
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
        }
        
        #endregion
    }
}
