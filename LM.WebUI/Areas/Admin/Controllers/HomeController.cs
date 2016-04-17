using System;
using System.Web.Mvc;
using LM.Model.Entity;
using LM.Model.Model;
using LM.Service;
using LM.Service.Security;

namespace LM.WebUI.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        //[Authentication]
        public ActionResult Index()
        {
            return RedirectToAction("HomeConfig");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(string userName, string userPwd)
        {
            var status = false;
            var loginMsg = "";
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(userPwd))
            {
                loginMsg = "请输入用户名和密码！";
            }
            else if (string.IsNullOrEmpty(userName))
            {
                loginMsg = "请输入用户名！";
            }
            else if (string.IsNullOrEmpty(userPwd))
            {
                loginMsg = "请输入密码！";
            }
            else
            {
                using (var userService = ResolveService<UserService>())
                {
                    var svs = userService.GetLoginUser(userName, userPwd);
                    if (svs.Status)
                    {
                        var user = svs.Data as User;
                        CurrentContext.SetUser(new CurrentUser { UserId = user.Id, UserName = user.Name });
                        status = true;
                    }
                    else
                    {
                        loginMsg = "用户名或密码错误！";
                    }
                }
            }

            return Json(new { status, loginMsg });
        }

        public ActionResult Logout()
        {
            // clear the session、cache、currentuser
            DiMySession.Clear();
            DiCache.RemoveAll();
            CurrentContext.ClearUser();
            return View("Login");
        }

        [Authentication]
        public ActionResult Top()
        {
            return View("_Top");
        }

        [Authentication]
        public ActionResult Left()
        {
            return View("_Left");
        }

        [HttpGet]
        //[Authentication]
        public ActionResult HomeConfig()
        {
            // 取现有的配置，首次为空
            using (var homePageConfigService = ResolveService<HomePageConfigService>())
            {
                var svs = homePageConfigService.GetLast();
                if (svs.Status)
                {
                    var config = svs.Data as HomePageConfigModel;
                    ViewBag.HomePageConfig = config;
                }
                else
                {
                    ViewBag.HomePageConfig = new HomePageConfigModel();
                }
                return View();
            }
        }

        [HttpPost]
        //[Authentication]
        public JsonResult HomeConfig(HomePageConfigModel info)
        {
            // 更新配置
            using (var homePageConfigService = ResolveService<HomePageConfigService>())
            {
                var rs = homePageConfigService.Insert(new HomePageConfig
                {
                    SiteName = info.SiteName,
                    LogoSrc = info.LogoSrc,
                    FlashSrc = info.FlashSrc,
                    AdSrc = info.AdSrc,
                    SloganSrc = info.SloganSrc,
                    QrSrc = info.QrSrc,
                    BusinessSrc = info.BusinessSrc,
                    SaveTime = DateTime.Now
                });

                var msg = rs.Status ? "保存成功！" : "服务器异常！";
                return Json(new { status = rs.Status, msg = msg });
            }
        }
        
        [HttpGet]
        //[Authentication]
        public ActionResult Preview(string a, string b, string c, string d, string e, string f, string g)
        {
            var config = new HomePageConfigModel
            {
                SiteName = a,
                LogoSrc = b,
                FlashSrc = c,
                AdSrc = d,
                SloganSrc = e,
                QrSrc = f,
                BusinessSrc = g
            };

            ViewBag.HomePageConfig = config;
            return View("_Preview");
        }

    }
}
