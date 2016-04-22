using System;
using System.Web.Mvc;
using LM.Model.Entity;
using LM.Model.Model;
using LM.Service;
using LM.Service.Base;
using LM.Service.Security;

namespace LM.WebUI.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        #region 登录登出
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

        #endregion


        #region 首页配置
        [HttpGet]
        //[Authentication]
        public ActionResult HomeConfig()
        {
            // 首先判断session中有无存储此配置信息
            var conf = DiMySession.Get<HomePageConfigModel>("HomePageConfig");
            if (conf != null)
            {
                ViewBag.HomePageConfig = conf;
                ViewBag.Nav = "Index";
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

                ViewBag.Nav = "Index";
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
                return Json(new JsonRespModel { status = rs.Status, message = msg });
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
        #endregion


        #region 联盟信息配置

        [HttpGet]
        public ActionResult BasicInfo()
        {
            // 取现有的配置，首次为空
            using (var baseInfoService = ResolveService<BaseInfoService>())
            {
                var svs = baseInfoService.GetLast();
                if (svs.Status)
                {
                    var config = svs.Data as BaseInfoModel;
                    ViewBag.BaseInfo = config;
                }
                else
                {
                    ViewBag.BaseInfo = new BaseInfoModel();
                }

                ViewBag.Nav = "BasicInfo";
                return View();
            }
        }


        [HttpPost]
        public JsonResult SaveBasicInfo(BaseInfoModel infoModel, string type)
        {
            using (var baseInfoService = ResolveService<BaseInfoService>())
            {
                var state = false;
                JsonRespModel respModel;
                // Id > 0 修改；Id = 0 新增
                if (infoModel.Id > 0)
                {
                    switch (type.ToLower())
                    {
                        case "contract":
                            state = baseInfoService.Update(new
                            {
                                Id = infoModel.Id,
                                Address = infoModel.Address,
                                Telphone = infoModel.Telphone,
                                Fax = infoModel.Fax,
                                Site = infoModel.Site,
                                Email = infoModel.Email,
                                Map = infoModel.Map,
                                ModifiedAt = DateTime.Now
                            }).Status;
                            break;
                        case "introduce":
                            state = baseInfoService.Update(new { Id = infoModel.Id, Introduce = infoModel.Introduce, ModifiedAt = DateTime.Now }).Status;
                            break;
                        case "chapter":
                            state = baseInfoService.Update(new { Id = infoModel.Id, Chapter = infoModel.Chapter, ModifiedAt = DateTime.Now }).Status;
                            break;
                        case "organize":
                            state = baseInfoService.Update(new { Id = infoModel.Id, Organize = infoModel.Organize, ModifiedAt = DateTime.Now }).Status;
                            break;
                        case "notice":
                            state = baseInfoService.Update(new { Id = infoModel.Id, Notice = infoModel.Notice, ModifiedAt = DateTime.Now }).Status;
                            break;
                        case "process":
                            state = baseInfoService.Update(new { Id = infoModel.Id, Process = infoModel.Process, ModifiedAt = DateTime.Now }).Status;
                            break;
                        default:
                            state = baseInfoService.Update(new { }).Status;
                            break;
                    }
                    respModel = new JsonRespModel { status = state, message = state ? "修改成功！" : "修改失败！" };
                }
                else
                {
                    BaseInfo baseInfo;
                    switch (type.ToLower())
                    {
                        case "contract":
                            baseInfo = new BaseInfo
                            {
                                Id = infoModel.Id,
                                Address = infoModel.Address,
                                Telphone = infoModel.Telphone,
                                Fax = infoModel.Fax,
                                Site = infoModel.Site,
                                Email = infoModel.Email,
                                Map = infoModel.Map,
                                ModifiedAt = DateTime.Now
                            };
                            break;
                        case "introduce":
                            baseInfo = new BaseInfo { Id = infoModel.Id, Introduce = infoModel.Introduce, ModifiedAt = DateTime.Now };
                            break;
                        case "chapter":
                            baseInfo = new BaseInfo { Id = infoModel.Id, Chapter = infoModel.Chapter, ModifiedAt = DateTime.Now };
                            break;
                        case "organize":
                            baseInfo = new BaseInfo { Id = infoModel.Id, Organize = infoModel.Organize, ModifiedAt = DateTime.Now };
                            break;
                        case "notice":
                            baseInfo = new BaseInfo { Id = infoModel.Id, Notice = infoModel.Notice, ModifiedAt = DateTime.Now };
                            break;
                        case "process":
                            baseInfo = new BaseInfo { Id = infoModel.Id, Process = infoModel.Process, ModifiedAt = DateTime.Now };
                            break;
                        default:
                            baseInfo = new BaseInfo();
                            break;
                    }
                    state = baseInfoService.Insert(baseInfo).Status;
                    respModel = new JsonRespModel { status = state, message = state ? "新建成功！" : "新建失败！" };
                }
                return Json(respModel);
            }
        }


        #endregion
    }
}
