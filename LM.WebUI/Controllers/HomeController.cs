using System.Collections.Generic;
using System.Web.Mvc;
using LM.Model.Entity;
using LM.Model.Model;
using LM.Service;
using LM.Service.Security;
using LM.Utility;

namespace LM.WebUI.Controllers
{
    public class HomeController : BaseController
    {
        #region 首页数据
        public ActionResult Index()
        {
            #region 首页配置数据
            // 首先判断session中有无存储此配置信息
            var conf = CurrentContext.Get("HomePageConfig");
            if (conf != null)
            {
                ViewBag.HomePageConfig = conf;
                return View();
            }
            // 若无，则取出后放入session
            using (var homePageConfigService = ResolveService<HomePageConfigService>())
            {
                var svs = homePageConfigService.GetLast();

                if (!svs.Status)
                {
                    CurrentContext.Set("HomePageConfig", new HomePageConfigModel());
                    ViewBag.Message = "未读取到如何首页配置信息！";
                    return View("Error");
                }

                var config = svs.Data as HomePageConfigModel;
                CurrentContext.Set("HomePageConfig", config);
            }
            #endregion




            return View();
        }

        #endregion


        #region 关于联盟 & 加入联盟

        public ActionResult About()
        {
            using (var baseInfoService = ResolveService<BaseInfoService>())
            {
                var baseInfo = baseInfoService.GetLast().Data as BaseInfoModel;
                ViewBag.Introduce = baseInfo.Introduce;
                return View();
            }
        }

        public ActionResult Chapter()
        {
            using (var baseInfoService = ResolveService<BaseInfoService>())
            {
                var baseInfo = baseInfoService.GetLast().Data as BaseInfoModel;
                ViewBag.Chapter = baseInfo.Chapter;
                return View();
            }
        }

        public ActionResult Organization()
        {
            using (var baseInfoService = ResolveService<BaseInfoService>())
            {
                var baseInfo = baseInfoService.GetLast().Data as BaseInfoModel;
                ViewBag.Organize = baseInfo.Organize;
                return View();
            }
        }

        public ActionResult Notice()
        {
            using (var baseInfoService = ResolveService<BaseInfoService>())
            {
                var baseInfo = baseInfoService.GetLast().Data as BaseInfoModel;
                ViewBag.Notice = baseInfo.Notice;
                return View();
            }
        }

        public ActionResult Process()
        {
            using (var baseInfoService = ResolveService<BaseInfoService>())
            {
                var baseInfo = baseInfoService.GetLast().Data as BaseInfoModel;
                ViewBag.Process = baseInfo.Process;
                return View();
            }
        }

        #endregion


        #region 联系我们

        public ActionResult Contact()
        {
            using (var baseInfoService = ResolveService<BaseInfoService>())
            {
                var info = baseInfoService.GetLast().Data as BaseInfoModel;
                info = info ?? new BaseInfoModel();
                ViewBag.Model = new BaseInfoModel { Address = info.Address, Telphone = info.Telphone, Fax = info.Fax, Site = info.Site, Email = info.Email, Map = info.Map };
                return View();
            }
        }

        #endregion

    }
}
