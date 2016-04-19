using System;
using LM.Component.Data;
using LM.Component.Data.Query;

namespace LM.Service.Base
{
    public class BaseService : IDisposable
    {
        public QueryManage QueryManage;

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
    }
}
