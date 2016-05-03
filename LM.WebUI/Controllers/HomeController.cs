using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LM.Model.Common;
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
            #region 页面所需数据

            var slideArticles = new List<ArticleModel>(); // 幻灯片数据
            var briefs = new List<BriefModel>(); // 3条简报
            var logos = new List<CompanyModel>(); // 10个logo数据
            var supCompanies = new List<CompanyModel>(); // 上游企业
            var subCompanies = new List<CompanyModel>(); // 下游企业
            var otherCompanies = new List<CompanyModel>(); // 科研机构
            var dynamicArticles = new List<ArticleModel>(); // 联盟动态
            var industryArticles = new List<ArticleModel>(); // 行业信息

            #endregion


            #region 首页配置数据
            // 首先判断session中有无存储此配置信息
            var conf = CurrentContext.Get("HomePageConfig");
            if (conf == null)
            {
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
            }
            #endregion


            #region 文章数据 Article
            using (var articleService = ResolveService<ArticleService>())
            {
                // 幻灯片数据
                var rs1 = articleService.GetList("select top 4 Id,Type,Title,ImgSrc from [Article] where IsHide = 0 and IsRecommend = 1 and ImgSrc is not null order by SaveAt desc,Title asc");
                if (rs1.Status && rs1.Data != null)
                {
                    slideArticles = rs1.Data as List<ArticleModel>;
                }
                // 联盟动态 
                var rs2 = articleService.GetList("select top 10 Id,Title from [Article] where IsHide = 0 and Type = '联盟动态' order by SaveAt desc");
                if (rs2.Status && rs2.Data != null)
                {
                   dynamicArticles = rs2.Data as List<ArticleModel>;
                }
                // 行业信息
                var rs3 = articleService.GetList("select top 10 Id,Title from [Article] where IsHide = 0 and Type = '行业信息' order by SaveAt desc");
                if (rs3.Status && rs3.Data != null)
                {
                   industryArticles = rs3.Data as List<ArticleModel>;
                }
            }

            #endregion


            #region 企业数据 Company
            using (var companyService = ResolveService<CompanyService>())
            {
                // logo
                var rs1 = companyService.GetList("select 20 Id,Name,LogoSrc,Site,SaveAt from [Company] order by Range asc,Name asc ");
                if (rs1.Status && rs1.Data != null)
                {
                    logos = rs1.Data as List<CompanyModel>;
                }
                // 上下游企业
                var rs2 = companyService.GetList("select Id,Name,SaveAt from [Company] order by Type asc");
                if (rs2.Status && rs2.Data != null)
                {
                    var comps = rs2.Data as List<CompanyModel>;
                    if (comps.Count > 0)
                    {
                        subCompanies = comps.Where(c => c.Type == MemberType.上游企业).ToList();
                        supCompanies = comps.Where(c => c.Type == MemberType.下游企业).ToList();
                        otherCompanies = comps.Where(c => c.Type == MemberType.科研院所协会).ToList();
                    }
                }
            }
            #endregion


            #region 简报数据 Brief，取最新3条
            using (var briefService = ResolveService<BriefService>())
            {
                var rs = briefService.GetList("select top 3 Id,Name,FilePath from [Brief] where order by SaveAt desc");
                if (rs.Status && rs.Data != null)
                {
                    briefs = rs.Data as List<BriefModel>;
                }
            }
            #endregion


            ViewBag.SlideArticles = slideArticles;
            ViewBag.Briefs = briefs;
            ViewBag.Logos = logos;
            ViewBag.SupCompanies = supCompanies;
            ViewBag.SubCompanies = subCompanies;
            ViewBag.OtherCompanies = otherCompanies;
            ViewBag.DynamicArticles = dynamicArticles;
            ViewBag.IndustryArticles = industryArticles;
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
