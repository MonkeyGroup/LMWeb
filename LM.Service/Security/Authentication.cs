using System;
using System.Reflection;
using System.Web.Mvc;
using LM.Model.Common;

namespace LM.Service.Security
{
    public class Authentication : FilterAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// 是否打开鉴权功能
        /// </summary>
        private readonly bool _openAuth;

        /// <summary>
        ///  当前方法所需的权限点
        /// </summary>
        public Function[] NeededFunctions { get; set; }

        /// <summary>
        ///  默认需要验证鉴权
        /// </summary>
        public Authentication()
        {
            _openAuth = true;
        }

        /// <summary>
        ///  默认需要验证鉴权
        /// </summary>
        /// <param name="openAuth"></param>
        public Authentication(bool openAuth = true)
        {
            _openAuth = openAuth;
            NeededFunctions = null;
        }

        /// <summary>
        /// 传入需要校验的权限集合
        /// </summary>
        /// <param name="functions">用户权限集合</param>
        public Authentication(Function[] functions)
        {
            NeededFunctions = functions;
            _openAuth = true;
        }

        /// <summary>
        ///  鉴权校验方法，
        ///  步骤：
        ///     1. 是否验证；
        ///     2. 登陆状态；
        ///     3. 是否有权限；
        ///     4. 其他校验。
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            // 1. 是否验证
            if (!_openAuth)
                return;

            // 2. 登陆状态
            var user = CurrentContext.GetCurrentUser();
            if (user == null || user.UserId == 0)
            {
                // 没有登录返回登录界面
                filterContext.Result = new RedirectResult("Login");
                return;
            }

            // 3. 权限验证
            if (NeededFunctions != null && !user.ContainFunctions(NeededFunctions))
            {
                // 无权操作
                var cr = new ContentResult();
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = 497;
                cr.Content = string.Format("<script type='text/javascript'>alert('{0}')</script>", "您无权操作！");
                filterContext.Result = cr;
            }

            // 4. 其他校验


        }

        /// <summary>
        ///  判断验证的action是否是异步的
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private bool IsAjaxAction(ControllerContext filterContext)
        {
            var clsType = filterContext.Controller.GetType();
            var action = filterContext.RouteData.Values["Action"] as string;
            var mInfo = clsType.GetMethod(action, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
            return mInfo != null && GetCustomAttribute(mInfo, typeof(AjaxMethodAttribute)) != null;
        }
    }


    /// <summary>
    /// 此特性标识此action是ajax的请求
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AjaxMethodAttribute : Attribute
    {
    }
}