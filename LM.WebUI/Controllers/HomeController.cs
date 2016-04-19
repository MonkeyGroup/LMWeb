using System.Collections.Generic;
using System.Web.Mvc;
using LM.Model.Entity;
using LM.Model.Model;
using LM.Service;
using LM.Utility;

namespace LM.WebUI.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            // 首先判断session中有无存储此配置信息
            var conf = DiMySession.Get<HomePageConfigModel>("HomePageConfig");
            if (conf != null)
            {
                ViewBag.HomePageConfig = conf;
                return View();
            }
            // 若无，则取出后放入session
            using (var homePageConfigService = ResolveService<HomePageConfigService>())
            {
                var svs = homePageConfigService.GetLast();
                if (svs.Status)
                {
                    var config = svs.Data as HomePageConfigModel;
                    ViewBag.HomePageConfig = config;
                    DiMySession.Set("HomePageConfig", config);
                }
                else
                {
                    ViewBag.HomePageConfig = new HomePageConfigModel();
                }
                return View();
            }
        }

        public ActionResult About()
        {
            using (var userService = ResolveService<UserService>())
            {
                //var sr = userService.GetByPage(1, 8);
                //var userList = new List<UserModel>();
                //((List<User>)sr.Data).ForEach<User>(u => { userList.Add(new UserModel { Id = u.Id, Pwd = u.Pwd, Name = u.Name }); });
                //ViewBag.UserList = userList;
                return View();
            }
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}
