using System;
using System.Collections.Generic;
using System.Web.Mvc;
using LM.Model.Common;
using LM.Model.Entity;
using LM.Model.Model;
using LM.Service;
using LM.WebUI.Areas.Admin.Models;

namespace LM.WebUI.Controllers
{
    public class CompanyController : BaseController
    {

        #region 联盟成员

        public ActionResult List(int type = 0, int pindex = 1)
        {
            //var models = new List<CompanyModel>();
            //var typeList = new List<CategoryModel>
            //{
            //    new CategoryModel{Id = (int)MemberType.材料领域企业, Name = MemberType.材料领域企业.ToString()},
            //    new CategoryModel{Id = (int)MemberType.应用领域企业, Name = MemberType.应用领域企业.ToString()},
            //    new CategoryModel{Id = (int)MemberType.科研协会理事单位, Name = MemberType.科研协会理事单位.ToString()},
            //};
            //int itemCount, psize;

            //// 获取列表数据
            //using (var companyService = ResolveService<CompanyService>())
            //{
            //    psize = 6;
            //    type = type == 0 ? (int)MemberType.材料领域企业 : type;
            //    var query = string.Format(@"(select a.Id, a.Name, a.LogoSrc, a.Site, a.SaveAt from [Company] a Where a.Type = {0})", type);
            //    var orderby = "SaveAt desc";
            //    var rs = companyService.GetByPage(query, orderby, pindex, psize, out itemCount);
            //    if (rs.Status && rs.Data != null)
            //    {
            //        models = rs.Data as List<CompanyModel>;
            //    }
            //}

            // 获取所有成员单位数据，并在页面按头衔进行分类
            var typeList = new List<CategoryModel>();
            using (var categoryService = ResolveService<CategoryService>())
            {
                var query = string.Format(@"select * from [Category] a Where a.Target = '{0}'", CategoryTarget.成员企业分类);
                var rs = categoryService.GetList(query);
                if (rs.Status && rs.Data != null)
                {
                    typeList = rs.Data as List<CategoryModel>;
                }
            }

            var models = new List<CompanyModel>();
            using (var companyService = ResolveService<CompanyService>())
            {
                //var query = @"select * from [Company]";
                var query = @"select a.*,b.[Index] ShowIndex  from [Company] a inner join [Category] b on a.Range = b.Id order by ShowIndex desc,Range desc ";
                var rs = companyService.GetList(query);
                if (rs.Status && rs.Data != null)
                {
                    models = rs.Data as List<CompanyModel>;
                }
            }

            #region 获取做成链接数据
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
            #endregion

            ViewBag.SlideArticles = slideArticles;
            ViewBag.DynamicArticles = dynamicArticles;
            ViewBag.Type = type;
            ViewBag.Companies = models;
            ViewBag.TypeList = typeList;
            //ViewBag.PageInfo = new PageInfo(pindex, psize, itemCount, (itemCount % psize == 0) ? (itemCount / psize) : (itemCount / psize + 1));
            return View();
        }

        public ActionResult Detail(int id)
        {
            CompanyModel model;
            using (var companyService = ResolveService<CompanyService>())
            {
                var rs = companyService.GetById(id);
                if (rs.Status && rs.Data != null)
                {
                    var entity = rs.Data as Company;
                    model = new CompanyModel
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        Site = entity.Site,
                        LogoSrc = entity.LogoSrc,
                        Description = entity.Description,
                        Type = entity.Type,
                        SaveAt = DateTime.Now,
                    };
                }
                else
                {
                    model = new CompanyModel();
                }
            }

            ViewBag.Company = model;
            return View();
        }

        #endregion

    }
}
