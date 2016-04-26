using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LM.Model.Entity;
using LM.Model.Model;
using LM.Service;
using LM.Service.Base;
using LM.Service.Security;
using LM.Utility;
using LM.WebUI.Areas.Admin.Models;

namespace LM.WebUI.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        #region 登录登出
        [Authentication]
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
                        userService.Update(new { Id = user.Id, LastLoginAt = DateTime.Now });
                        CurrentContext.SetUser(new CurrentUser { UserId = user.Id, UserName = user.Name });
                        status = true;
                        loginMsg = "登录成功！";
                    }
                    else
                    {
                        loginMsg = "用户名或密码错误！";
                    }
                    userService.WriteLog(new OperationLog { User = userName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", loginMsg, "登录后台系统") });
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
        [Authentication]
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

                homePageConfigService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", svs.Status ? "查看成功！" : "查看失败！", "查看首页配置页") });

                ViewBag.Nav = "Index";
                return View();
            }
        }

        [HttpPost]
        [Authentication]
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
                    QrSrc = info.QrSrc,
                    AdSrc = info.AdSrc,
                    AdLink = info.AdLink,
                    SloganSrc = info.SloganSrc,
                    SloganLink = info.SloganLink,
                    BusinessSrc = info.BusinessSrc,
                    BusinessLink = info.BusinessLink,
                    Product1Src = info.Product1Src,
                    Product1Link = info.Product1Link,
                    Product2Src = info.Product2Src,
                    Product2Link = info.Product2Link,
                    Product3Src = info.Product3Src,
                    Product3Link = info.Product3Link,
                    Product4Src = info.Product4Src,
                    Product4Link = info.Product4Link,
                    SaveAt = DateTime.Now
                });
                if (rs.Status)
                {
                    DiMySession.Clear("HomePageConfig");
                    DiMySession.Set("HomePageConfig", info);
                }

                homePageConfigService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", rs.Status ? "修改成功！" : "修改失败！", "修改首页配置页") });

                var msg = rs.Status ? "保存成功！" : "服务器异常！";
                return Json(new JsonRespModel { status = rs.Status, message = msg });
            }
        }

        [HttpGet]
        [Authentication]
        public ActionResult Preview(string a, string b, string c, string d, string e, string f, string g, string h, string i, string j, string k)
        {
            var config = new HomePageConfigModel
            {
                SiteName = a,
                LogoSrc = b,
                FlashSrc = c,
                AdSrc = d,
                SloganSrc = e,
                QrSrc = f,
                BusinessSrc = g,
                Product1Src = h,
                Product2Src = i,
                Product3Src = j,
                Product4Src = k
            };

            ViewBag.HomePageConfig = config;
            return View("_Preview");
        }
        #endregion


        #region 联盟信息配置

        [HttpGet]
        [Authentication]
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

                baseInfoService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", svs.Message, "查看系统配置信息") });

                ViewBag.Nav = "BasicInfo";
                return View();
            }
        }


        [HttpPost]
        [Authentication]
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
                                SaveAt = DateTime.Now
                            }).Status;
                            break;
                        case "introduce":
                            state = baseInfoService.Update(new { Id = infoModel.Id, Introduce = infoModel.Introduce, SaveAt = DateTime.Now }).Status;
                            break;
                        case "chapter":
                            state = baseInfoService.Update(new { Id = infoModel.Id, Chapter = infoModel.Chapter, SaveAt = DateTime.Now }).Status;
                            break;
                        case "organize":
                            state = baseInfoService.Update(new { Id = infoModel.Id, Organize = infoModel.Organize, SaveAt = DateTime.Now }).Status;
                            break;
                        case "notice":
                            state = baseInfoService.Update(new { Id = infoModel.Id, Notice = infoModel.Notice, SaveAt = DateTime.Now }).Status;
                            break;
                        case "process":
                            state = baseInfoService.Update(new { Id = infoModel.Id, Process = infoModel.Process, SaveAt = DateTime.Now }).Status;
                            break;
                        default:
                            state = baseInfoService.Update(new { }).Status;
                            break;
                    }
                    baseInfoService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", state ? "修改成功！" : "修改失败！", "修改系统配置信息") });
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
                                SaveAt = DateTime.Now
                            };
                            break;
                        case "introduce":
                            baseInfo = new BaseInfo { Id = infoModel.Id, Introduce = infoModel.Introduce, SaveAt = DateTime.Now };
                            break;
                        case "chapter":
                            baseInfo = new BaseInfo { Id = infoModel.Id, Chapter = infoModel.Chapter, SaveAt = DateTime.Now };
                            break;
                        case "organize":
                            baseInfo = new BaseInfo { Id = infoModel.Id, Organize = infoModel.Organize, SaveAt = DateTime.Now };
                            break;
                        case "notice":
                            baseInfo = new BaseInfo { Id = infoModel.Id, Notice = infoModel.Notice, SaveAt = DateTime.Now };
                            break;
                        case "process":
                            baseInfo = new BaseInfo { Id = infoModel.Id, Process = infoModel.Process, SaveAt = DateTime.Now };
                            break;
                        default:
                            baseInfo = new BaseInfo();
                            break;
                    }
                    state = baseInfoService.Insert(baseInfo).Status;
                    baseInfoService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", state ? "新建成功！" : "新建失败！", "新建系统配置信息") });
                    respModel = new JsonRespModel { status = state, message = state ? "新建成功！" : "新建失败！" };
                }
                return Json(respModel);
            }
        }


        #endregion


        #region 用户模块

        [HttpGet]
        [Authentication]
        public ActionResult UserList(int pindex = 1, int psize = 2)
        {
            List<UserModel> models;
            int itemCount;

            using (var userService = ResolveService<UserService>())
            {
                psize = AppSettingHelper.GetInt("PageSize") != 0 ? AppSettingHelper.GetInt("PageSize") : psize;
                var rs = userService.GetByPage("[User]", "SaveAt desc", pindex, psize, out itemCount);
                models = rs.Status ? rs.Data as List<UserModel> : new List<UserModel>();

                userService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", rs.Status ? "查询成功！" : "查询失败！", "用户列表页") });
            }

            ViewBag.Users = models;
            ViewBag.PageInfo = new PageInfo(pindex, psize, itemCount, (itemCount % psize == 0) ? (itemCount / psize) : (itemCount / psize + 1));
            ViewBag.Nav = "UserList";
            return View();
        }

        [HttpGet]
        [Authentication]
        public ActionResult User(int id = 0)
        {
            var model = new UserModel();

            // Id > 0 是编辑；Id = 0 是新建
            if (id > 0)
            {
                using (var userService = ResolveService<UserService>())
                {
                    var rs = userService.GetById(id);
                    if (rs.Status && rs.Data != null)
                    {
                        var entity = rs.Data as User;
                        model = new UserModel
                        {
                            Id = entity.Id,
                            Name = entity.Name
                        };
                    }
                }
            }

            ViewBag.User = model;
            ViewBag.Nav = "UserList";
            return View();
        }

        [HttpPost]
        [Authentication]
        public JsonResult SaveUser(UserModel model)
        {
            // 若无，则取出后放入session
            using (var userService = ResolveService<UserService>())
            {
                // Id > 0 修改，Id = 0 新增
                JsonRespModel respModel;
                if (model.Id > 0)
                {
                    var svs = userService.Update(new User
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Pwd = model.Pwd.Md5String()
                    });
                    respModel = new JsonRespModel { status = svs.Status, message = svs.Status ? "修改成功！" : "修改失败！" };
                    userService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", svs.Status ? "修改成功！" : "修改失败！", "用户修改页") });
                }
                else
                {
                    var svs = userService.Insert(new User
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Pwd = model.Pwd.Md5String(),
                        SaveAt = DateTime.Now,
                        LastLoginAt = DateTime.Now,
                    });
                    respModel = new JsonRespModel { status = svs.Status, message = svs.Status ? "新建成功！" : "新建失败！" };
                    userService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", svs.Status ? "新建成功！" : "新建失败！", "用户新建页") });
                }

                return Json(respModel);
            }
        }

        [HttpGet]
        [Authentication]
        public ActionResult DeleteUser(string ids)
        {
            using (var userService = ResolveService<UserService>())
            {
                var rs = userService.Delete(ids.Split(',').Select(int.Parse).ToArray());
                userService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", rs.Status ? "删除成功！" : "删除失败！", "用户删除页") });
                return RedirectToAction("UserList");
            }
        }

        #endregion


        #region 日志模块

        [HttpGet]
        public ActionResult OpLogList(int pindex = 1, int psize = 2)
        {
            List<OperationLogModel> models;
            int itemCount;

            using (var operationLogService = ResolveService<OperationLogService>())
            {
                psize = AppSettingHelper.GetInt("PageSize") != 0 ? AppSettingHelper.GetInt("PageSize") : psize;
                const string query = @"[OperationLog]";
                var rs = operationLogService.GetByPage(query, "SaveAt desc", pindex, psize, out itemCount);
                models = rs.Status ? rs.Data as List<OperationLogModel> : new List<OperationLogModel>();
                operationLogService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", rs.Message, "查看日志") });
            }

            ViewBag.OperationLogs = models;
            ViewBag.PageInfo = new PageInfo(pindex, psize, itemCount, (itemCount % psize == 0) ? (itemCount / psize) : (itemCount / psize + 1));
            ViewBag.Nav = "OpLogList";
            return View();
        }


        #endregion


    }
}
