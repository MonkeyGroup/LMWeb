using System;
using LM.Component.Data;
using LM.Component.Data.Query;
using LM.Model.Entity;

namespace LM.Service.Base
{
    public class BaseService : IDisposable
    {
        protected QueryManage QueryManage;

        public BaseService() { }

        public BaseService(DbSession dbSession)
        {
            QueryManage = new QueryManage(dbSession);
        }

        public void Dispose()
        {
            if (QueryManage == null) return;
            QueryManage.DbSession.Dispose();
            QueryManage = null;
        }

        public void WriteLog(OperationLog log)
        {
            log.SaveAt = DateTime.Now;
            const string noSql = "insert into [OperationLog] ([User],[Operation],[Ip],[SaveAt]) values (@User, @Operation, @Ip, @SaveAt)";
            QueryManage.GetExecuteNoQuery(noSql, log);
        }
    }
}
