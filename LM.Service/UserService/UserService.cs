using System;

using LM.Component.Data;
using LM.Model.Entity;
using LM.Model.Model;
using LM.Repo.Repo;

namespace LM.Service.UserService
{
    public class UserService : BaseService
    {
        private readonly UserRepo _userRepo;

        public UserService(IUnitOfWork uw)
            : base(uw)
        {
            _userRepo = new UserRepo(uw);
        }

        #region 用户模块的相关方法
        public ServiceResult GetUserList()
        {
            try
            {
                var userList = QueryManage.GetList<UserModel>("select * from dbo.[User]");

                return new ServiceResult(true, ServiceResultCode.正常) { Data = userList };
            }
           catch (Exception e)
            {
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
        }
        #endregion

    }
}
