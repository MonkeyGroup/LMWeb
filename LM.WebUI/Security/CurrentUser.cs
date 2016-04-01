using System;
using System.Linq;
using LM.WebUI.Common;

namespace LM.WebUI.Security
{
    /// <summary>
    /// 当前登录用户
    /// </summary>
    [Serializable]
    public class CurrentUser
    {
        public long UserId { get; set; }

        // 用户名，登陆账号
        public string UserName { get; set; }

        /// <summary>
        ///  权限
        /// </summary>
        public Function[] Functions { get; set; }

        /// <summary>
        ///  当前用户是否具有权限。
        /// </summary>
        /// <param name="func">判断的权限集合</param>
        /// <returns></returns>
        public bool ContainFunctions(Function[] func)
        {
            if (Functions != null)
            {
                if (Functions.Intersect(func).Any())
                {
                    return true;
                }
            }
            return false;
        }

    }
}