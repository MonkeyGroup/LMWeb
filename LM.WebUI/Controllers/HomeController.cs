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
            #region ViewModels 页面所需数据

            var oneNews = new ArticleModel();
            var oneFocus = new ArticleModel();
            var slideArticles = new List<ArticleModel>(); // 幻灯片数据
            var briefs = new List<BriefModel>(); // 3条简报
            var logos = new List<CompanyModel>(); // 10个logo数据
            var ads = new List<AdModel>(); // 10个广告数据
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
                        CurrentContext.Set("HomePageConfig", new HomePageConfigModel(), 120);
                        ViewBag.Message = "未读取到任何首页配置信息！";
                        return View("Error");
                    }

                    var config = svs.Data as HomePageConfigModel;
                    CurrentContext.Set("HomePageConfig", config, 120);
                }
            }
            #endregion


            #region Footer 数据
            var footerModel = CurrentContext.Get("FooterModel");
            if (footerModel == null)
            {
                var model = new FooterModel();

                using (var baseInfoService = ResolveService<BaseInfoService>())
                {
                    var rs = baseInfoService.GetLast();
                    if (rs.Status && rs.Data != null)
                    {
                        var info = rs.Data as BaseInfoModel;
                        model.Address = info.Address;
                        model.Email = info.Email;
                        model.Telphone = info.Telphone;
                    }
                }

                using (var companyService = ResolveService<CompanyService>())
                {
                    var rs = companyService.GetList("select Id,Name,RangeName,Site from [Company] order by Range asc,Id asc");
                    if (rs.Status && rs.Data != null)
                    {
                        model.companies = rs.Data as List<CompanyModel>;
                    }
                }

                CurrentContext.Set("FooterModel", model, 720);
            }
            #endregion


            #region 文章数据 Article
            using (var articleService = ResolveService<ArticleService>())
            {
                // 联盟要闻
                var rs = articleService.GetList("select top 1 Id,Title,Brief from [Article] where Type = '联盟动态' and IsRecommend = 1 order by SaveAt desc,Id desc");
                if (rs.Status && rs.Data != null)
                {
                    oneNews = (rs.Data as List<ArticleModel>).FirstOrDefault();
                }
                // 特别关注
                var rs0 = articleService.GetList("select top 1 Id,Title,Brief from [Article] where Type = '行业信息' and IsFocus = 1 order by  SaveAt desc,Id desc");
                if (rs0.Status && rs0.Data != null)
                {
                    oneFocus = (rs0.Data as List<ArticleModel>).FirstOrDefault();
                }
                // 幻灯片数据
                var rs1 = articleService.GetList("select top 4 Id,Type,Title,ImgSrc from [Article] where IsShow = 1 and ImgSrc is not null order by SaveAt desc, Id desc");
                if (rs1.Status && rs1.Data != null)
                {
                    slideArticles = rs1.Data as List<ArticleModel>;
                }
                // 联盟动态 
                var rs2 = articleService.GetList("select top 10 Id,Title,SaveAt from [Article] where Type = '联盟动态' order by  SaveAt desc,Id desc");
                if (rs2.Status && rs2.Data != null)
                {
                    dynamicArticles = rs2.Data as List<ArticleModel>;
                }
                // 行业信息
                var rs3 = articleService.GetList("select top 10 Id,Title,SaveAt from [Article] where Type = '行业信息' order by SaveAt desc, Id desc");
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
                var rs1 = companyService.GetList("select top 20 Id,Name,LogoSrc,Site,SaveAt from [Company] order by Range asc,Name asc ");
                if (rs1.Status && rs1.Data != null)
                {
                    logos = rs1.Data as List<CompanyModel>;
                }
                // 上下游企业
                var rs2 = companyService.GetList("select Id,Name,Type,Site,SaveAt from [Company] order by Type asc");
                if (rs2.Status && rs2.Data != null)
                {
                    var comps = rs2.Data as List<CompanyModel>;
                    if (comps.Count > 0)
                    {
                        subCompanies = comps.Where(c => c.Type == MemberType.材料领域企业).ToList();
                        supCompanies = comps.Where(c => c.Type == MemberType.应用领域企业).ToList();
                        otherCompanies = comps.Where(c => c.Type == MemberType.科研协会理事单位).ToList();
                    }
                }
            }
            #endregion


            #region 简报数据 Brief，取最新3条
            using (var briefService = ResolveService<BriefService>())
            {
                var rs = briefService.GetList("select top 3 Id,Name,FilePath from [Brief] order by SaveAt desc");
                if (rs.Status && rs.Data != null)
                {
                    briefs = rs.Data as List<BriefModel>;
                }
            }
            #endregion


            #region 广告数据，取最新10条
            using (var adService = ResolveService<AdService>())
            {
                var rs = adService.GetList("select top 10 Id,Name,ImgSrc,LinkUrl from [Ad] order by [Index] desc,Id desc");
                if (rs.Status && rs.Data != null)
                {
                    ads = rs.Data as List<AdModel>;
                }
            }
            #endregion
            

            ViewBag.OneNews = oneNews;
            ViewBag.OneFocus = oneFocus;
            ViewBag.SlideArticles = slideArticles;
            ViewBag.Briefs = briefs;
            ViewBag.Logos = logos;
            ViewBag.Ads = ads;
            ViewBag.SupCompanies = supCompanies;
            ViewBag.SubCompanies = subCompanies;
            ViewBag.OtherCompanies = otherCompanies;
            ViewBag.DynamicArticles = dynamicArticles;
            ViewBag.IndustryArticles = industryArticles;
            return View();
        }

        #endregion


        #region 搜索

        public ActionResult Search(string key)
        {
            #region 返回数据

            var articles = new List<ArticleModel>();
            var companies = new List<CompanyModel>();
            var products = new List<ProductModel>();

            #endregion


            #region 查找文章
            using (var articleService = ResolveService<ArticleService>())
            {
                var query = string.Format(@"select top 10 Id,Title,Brief from [Article] where CHARINDEX('{0}',Title)>0 order by SaveAt desc", key);
                var rs = articleService.GetList(query);
                if (rs.Status && rs.Data != null)
                {
                    articles = rs.Data as List<ArticleModel>;
                }
            }
            #endregion


            #region 查找企业

            using (var companyService = ResolveService<CompanyService>())
            {
                var query = string.Format(@"select top 10 Id,Name,Description from [Company] where CHARINDEX('{0}',Name)>0 order by SaveAt desc", key);
                var rs = companyService.GetList(query);
                if (rs.Status && rs.Data != null)
                {
                    companies = rs.Data as List<CompanyModel>;
                }
            }
            #endregion


            #region 查找产品

            using (var productService = ResolveService<ProductService>())
            {
                var query = string.Format(@"select top 10 Id,Name,Description from [Product] where CHARINDEX('{0}',Name)>0 order by SaveAt desc", key);
                var rs = productService.GetList(query);
                if (rs.Status && rs.Data != null)
                {
                    products = rs.Data as List<ProductModel>;
                }
            }
            #endregion


            ViewBag.Articles = articles;
            ViewBag.Companies = companies;
            ViewBag.Products = products;
            return View();
        }
        #endregion


        #region 关于联盟 & 加入联盟

        public ActionResult About()
        {
            var slideArticles = new List<ArticleModel>(); // 幻灯片数据
            var dynamicArticles = new List<ArticleModel>(); // 联盟动态

            using (var articleService = ResolveService<ArticleService>())
            {
                // 幻灯片数据
                var rs1 = articleService.GetList("select top 4 Id,Type,Title,ImgSrc from [Article] where IsShow = 1 and ImgSrc is not null order by SaveAt desc, Id desc");
                if (rs1.Status && rs1.Data != null)
                {
                    slideArticles = rs1.Data as List<ArticleModel>;
                }
                // 联盟动态 
                var rs2 = articleService.GetList("select top 10 Id,Title,SaveAt from [Article] where Type = '联盟动态' order by  SaveAt desc,Id desc");
                if (rs2.Status && rs2.Data != null)
                {
                    dynamicArticles = rs2.Data as List<ArticleModel>;
                }
            }

            using (var baseInfoService = ResolveService<BaseInfoService>())
            {
                var baseInfo = baseInfoService.GetLast().Data as BaseInfoModel;
                ViewBag.Introduce = baseInfo.Introduce;
            }

            ViewBag.SlideArticles = slideArticles;
            ViewBag.DynamicArticles = dynamicArticles;
            return View();
        }

        public ActionResult Chapter()
        {
            var briefs = new List<BriefModel>(); // 3条简报

            using (var briefService = ResolveService<BriefService>())
            {
                var rs = briefService.GetList("select top 3 Id,Name,FilePath from [Brief] order by SaveAt desc");
                if (rs.Status && rs.Data != null)
                {
                    briefs = rs.Data as List<BriefModel>;
                }
            }

            using (var baseInfoService = ResolveService<BaseInfoService>())
            {
                var baseInfo = baseInfoService.GetLast().Data as BaseInfoModel;
                ViewBag.Chapter = baseInfo.Chapter;
            }

            ViewBag.Briefs = briefs;
            return View();
        }

        public ActionResult Organization()
        {
            var briefs = new List<BriefModel>(); // 3条简报

            using (var briefService = ResolveService<BriefService>())
            {
                var rs = briefService.GetList("select top 3 Id,Name,FilePath from [Brief] order by SaveAt desc");
                if (rs.Status && rs.Data != null)
                {
                    briefs = rs.Data as List<BriefModel>;
                }
            }

            using (var baseInfoService = ResolveService<BaseInfoService>())
            {
                var baseInfo = baseInfoService.GetLast().Data as BaseInfoModel;
                ViewBag.Organize = baseInfo.Organize;
            }

            ViewBag.Briefs = briefs;
            return View();
        }

        public ActionResult Notice()
        {
            using (var baseInfoService = ResolveService<BaseInfoService>())
            {
                var baseInfo = baseInfoService.GetLast().Data as BaseInfoModel;
                ViewBag.Notice = baseInfo.Notice;
                ViewBag.ApplyFilePath = baseInfo.ApplyFilePath;
                return View();
            }
        }

        public ActionResult Process()
        {
            var slideArticles = new List<ArticleModel>(); // 幻灯片数据
            var briefs = new List<BriefModel>(); // 3条简报
            var dynamicArticles = new List<ArticleModel>(); // 联盟动态

            using (var articleService = ResolveService<ArticleService>())
            {
                // 幻灯片数据
                var rs1 = articleService.GetList("select top 4 Id,Type,Title,ImgSrc from [Article] where IsShow = 1 and ImgSrc is not null order by SaveAt desc, Id desc");
                if (rs1.Status && rs1.Data != null)
                {
                    slideArticles = rs1.Data as List<ArticleModel>;
                }
                // 联盟动态 
                var rs2 = articleService.GetList("select top 10 Id,Title,SaveAt from [Article] where Type = '联盟动态' order by  SaveAt desc,Id desc");
                if (rs2.Status && rs2.Data != null)
                {
                    dynamicArticles = rs2.Data as List<ArticleModel>;
                }
            }

            using (var briefService = ResolveService<BriefService>())
            {
                var rs = briefService.GetList("select top 3 Id,Name,FilePath from [Brief] order by SaveAt desc");
                if (rs.Status && rs.Data != null)
                {
                    briefs = rs.Data as List<BriefModel>;
                }
            }

            using (var baseInfoService = ResolveService<BaseInfoService>())
            {
                var baseInfo = baseInfoService.GetLast().Data as BaseInfoModel;
                ViewBag.Process = baseInfo.Process;
            }

            ViewBag.SlideArticles = slideArticles;
            ViewBag.Briefs = briefs;
            ViewBag.DynamicArticles = dynamicArticles;
            return View();
        }

        #endregion


        #region 联系我们

        public ActionResult Contact()
        {
            var slideArticles = new List<ArticleModel>(); // 幻灯片数据
            var dynamicArticles = new List<ArticleModel>(); // 联盟动态

            using (var articleService = ResolveService<ArticleService>())
            {
                // 幻灯片数据
                var rs1 = articleService.GetList("select top 4 Id,Type,Title,ImgSrc from [Article] where IsShow = 1 and ImgSrc is not null order by SaveAt desc, Id desc");
                if (rs1.Status && rs1.Data != null)
                {
                    slideArticles = rs1.Data as List<ArticleModel>;
                }
                // 联盟动态 
                var rs2 = articleService.GetList("select top 10 Id,Title,SaveAt from [Article] where Type = '联盟动态' order by  SaveAt desc,Id desc");
                if (rs2.Status && rs2.Data != null)
                {
                    dynamicArticles = rs2.Data as List<ArticleModel>;
                }
            }

            using (var baseInfoService = ResolveService<BaseInfoService>())
            {
                var info = baseInfoService.GetLast().Data as BaseInfoModel;
                info = info ?? new BaseInfoModel();
                ViewBag.Model = new BaseInfoModel { Address = info.Address, Telphone = info.Telphone, Fax = info.Fax, Site = info.Site, Email = info.Email, Map = info.Map };
            }

            ViewBag.SlideArticles = slideArticles;
            ViewBag.DynamicArticles = dynamicArticles;
            return View();
        }

        #endregion

    }
}
