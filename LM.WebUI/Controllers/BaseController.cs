using System;
using System.Web.Mvc;
using LM.Service;
using LM.Service.BootStrapIoC;
using LM.Service.Security;
using LM.Utility.Util;
using Microsoft.Practices.Unity;

namespace LM.WebUI.Controllers
{
    /// <summary>
    ///  从IoC容器中取出所需实例对象
    /// </summary>
    public class BaseController : Controller
    {

        #region List all the Injections
        [Dependency]
        public ISession DiMySession { get; set; }

        [Dependency]
        public ICache DiCache { get; set; }

        //[Dependency]
        //public UserService DiUserService { get; set; }


        #endregion


        public CurrentUser CurrentUser;

        public BaseController()
        {
            CurrentUser = CurrentContext.GetCurrentUser();
        }

        /// <summary>
        ///  从 AutoFac 的 IoC 容器中取出service层实例
        /// </summary>
        /// <typeparam name="T">Service 类型</typeparam>
        /// <param name="serviceName">Service 在IoC注入时的标记名</param>
        /// <returns>Service 实例对象</returns>
        public T ResolveService<T>(string serviceName = "")
        {
            var service = string.IsNullOrEmpty(serviceName) ? UnityBootStrapper.Instance.UnityContainer.Resolve<T>()
                : UnityBootStrapper.Instance.UnityContainer.Resolve<T>(serviceName);
            if (service != null) return service;

            var msg = string.Format("在获取的Service层方法：{0}  时，出现异常！", serviceName);
            throw new Exception(msg);
        }
    }
}
