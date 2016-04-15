using System;
using System.Collections.Generic;
using System.Linq;
using LM.Component.Data;
using LM.Model.Entity;
using LM.Repo.Repo;
using LM.Service.Base;

namespace LM.Service
{
    public class UserService : BaseService
    {
        private readonly UserRepo _userRepo;

        public UserService(DbSession dbSession)
            : base(dbSession)
        {
            _userRepo = new UserRepo(dbSession);
        }


        #region CRUD操作

        public ServiceResult Insert(User user)
        {
            try
            {
                user.Id = _userRepo.Insert(user);
                return user.Id > 0 ? new ServiceResult(true, ServiceResultCode.正常, "成功") : new ServiceResult(false, ServiceResultCode.服务器异常, ServiceResultCode.服务器异常.ToString());
            }
            catch (Exception e)
            {
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
        }

        public ServiceResult Insert(IEnumerable<User> users)
        {
            try
            {
                var state = _userRepo.Insert(users);
                return state ? new ServiceResult(true, ServiceResultCode.正常, "成功") : new ServiceResult(false, ServiceResultCode.服务器异常, ServiceResultCode.服务器异常.ToString());
            }
            catch (Exception e)
            {
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
        }

        public ServiceResult Update(object novel)
        {
            try
            {
                var state = _userRepo.Update(novel);
                return state ? new ServiceResult(true, ServiceResultCode.正常, "成功") : new ServiceResult(false, ServiceResultCode.服务器异常, ServiceResultCode.服务器异常.ToString());
            }
            catch (Exception e)
            {
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
        }

        public ServiceResult Delete(int id)
        {
            try
            {
                var state = _userRepo.Delete(new User { Id = id });
                return state ? new ServiceResult(true, ServiceResultCode.正常, "成功") : new ServiceResult(false, ServiceResultCode.服务器异常, ServiceResultCode.服务器异常.ToString());
            }
            catch (Exception e)
            {
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
        }

        public ServiceResult Delete(int[] ids)
        {
            try
            {
                var state = _userRepo.Delete(ids.Distinct());
                return state ? new ServiceResult(true, ServiceResultCode.正常, "成功") : new ServiceResult(false, ServiceResultCode.服务器异常, ServiceResultCode.服务器异常.ToString());
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
                var novel = _userRepo.Get(id);
                return new ServiceResult(true, ServiceResultCode.正常, "成功", novel);
            }
            catch (Exception e)
            {
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
        }

        public ServiceResult GetByPage(int pageIndex, int pageSize)
        {
            try
            {
                const string query = "[User]";
                int pageCount;
                var novels = QueryManage.GetListByPage<User>(query, out pageCount, pageIndex, pageSize).ToList();
                return new ServiceResult(true) { Data = novels };
            }
            catch (Exception e)
            {
                return new ServiceResult(false, ServiceResultCode.服务器异常, e.Message);
            }
        }

        #endregion

    }
}
