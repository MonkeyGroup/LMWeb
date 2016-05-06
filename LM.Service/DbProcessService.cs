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

        public void RestoreDb(string dbName, string filePath)
        {
            var query = string.Format(@"
USE master;
ALTER DATABASE {0}
SET SINGLE_USER
WITH ROLLBACK IMMEDIATE;
ALTER DATABASE {0}
SET READ_ONLY;
use master restore database {0} from disk='{1}' with REPLACE;
ALTER DATABASE {0}
SET MULTI_USER", dbName, filePath);
            QueryManage.GetExecuteNoQuery(query);
        }

        public void BackupDb(string dbName, string filePath)
        {
            var query = string.Format(@"use master backup database {0} To disk='{1}' With init", dbName, filePath);
            QueryManage.GetExecuteNoQuery(query);
        }

    }
}
