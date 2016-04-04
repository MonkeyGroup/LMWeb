using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using LM.Component.Data;
using LM.Component.Data.Query;
using LM.Component.Data.Repo;
using LM.Model.Entity;

namespace LM.Repo.Repo
{
    public class UserRepo : BaseRepository<User>
    {
        public UserRepo(IUnitOfWork uw)
            : base(uw)
        {
        }


        #region 复杂的 CRUD 方法


        #endregion

    }
}
