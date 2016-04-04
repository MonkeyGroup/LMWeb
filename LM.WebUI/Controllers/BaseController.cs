using System.Web.Mvc;
using LM.Component.Data;
using LM.Service.UserService;
using LM.Utility.Util;
using LM.WebUI.Security;
using Microsoft.Practices.Unity;

namespace LM.WebUI.Controllers
{
    /// <summary>
    ///  从IoC容器中取出所需实例对象
    /// </summary>
    public class BaseController : Controller
    {
        public CurrentUser CurrentUser;

        public BaseController()
        {
            CurrentUser = CurrentContext.GetCurrentUser();
        }

        [Dependency]
        public ISession DiMySession { get; set; }

        [Dependency]
        public ICookie DiCookie { get; set; }

        [Dependency]
        public ICache DiCache { get; set; }

        [Dependency("WriteUnitOfWork")]
        public IUnitOfWork UnitOfWork { get; set; }

        [Dependency("ReadUnitOfWork")]
        public IUnitOfWork ReadUnitOfWork { get; set; }


    }
}
