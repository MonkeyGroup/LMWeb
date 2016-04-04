using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LM.Component.Data;
using LM.Component.Data.Query;

namespace LM.Service
{
    public class BaseService
    {
        public QueryManage QueryManage;

        public BaseService() { }

        public BaseService(IUnitOfWork uw)
        {
            QueryManage = new QueryManage(uw);
        }
    }
}
