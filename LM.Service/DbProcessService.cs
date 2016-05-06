using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LM.Service.Base;
using LM.Component.Data;

namespace LM.Service
{
    public class DbProcessService : BaseService
    {
        public DbProcessService(DbSession dbSession) : base(dbSession) { }

        public bool RestoreDb(string dbName, string filePath)
        {
            try
            {
                var query = string.Format(@"use master restore database {0} from disk ='{1}' with Recovery", dbName, filePath);
                var rs = QueryManage.GetExecuteNoQuery(query);
                return rs > 0;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool BackupDb(string dbName, string filePath)
        {
            try
            {
                var query = string.Format(@"use master backup database {0} To disk='{1}' With init", dbName, filePath);
                QueryManage.GetExecuteNoQuery(query);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}
