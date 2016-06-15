using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LM.Model.Common;
using LM.Model.Entity;
using LM.Model.Model;
using LM.Service;
using LM.WebUI.Areas.Admin.Models;

namespace LM.WebUI.Controllers
{
    public class ProductController : BaseController
    {

        #region 联盟产品

        public ActionResult List(int type = 0, int pindex = 1)
        {
            var models = new List<ProductModel>();
            var typeList = new List<CategoryModel>();
            int itemCount, psize;

            // 获取分类下拉框数据
            using (var categoryService = ResolveService<CategoryService>())
            {
                var rs = categoryService.GetByTarget(target: CategoryTarget.产品分类.ToString());
                if (rs.Status && rs.Data != null)
                {
                    typeList = rs.Data as List<CategoryModel>;
                }
            }

            // 获取列表数据
            using (var productService = ResolveService<ProductService>())
            {
                psize = 10;
                type = type == 0 && typeList.Count > 0 ? typeList.First().Id : type;
                var query = string.Format(@"(select a.Id, a.Name, a.ImgSrc,a.Level,a.State, a.Application, a.Company,a.SaveAt from [Product] a Where a.Type = {0})", type);
                var orderby = "SaveAt desc";
                var rs = productService.GetByPage(query, orderby, pindex, psize, out itemCount);
                if (rs.Status && rs.Data != null)
                {
                    models = rs.Data as List<ProductModel>;
                }
            }

            var briefs = new List<BriefModel>(); // 3条简报

            using (var briefService = ResolveService<BriefService>())
            {
                var rs = briefService.GetList("select top 3 Id,Name,FilePath from [Brief] order by SaveAt desc");
                if (rs.Status && rs.Data != null)
                {
                    briefs = rs.Data as List<BriefModel>;
                }
            }

            ViewBag.Briefs = briefs;
            ViewBag.Type = type;
            ViewBag.Products = models;
            ViewBag.TypeList = typeList;
            ViewBag.PageInfo = new PageInfo(pindex, psize, itemCount, (itemCount % psize == 0) ? (itemCount / psize) : (itemCount / psize + 1));
            return View("List");
        }

        public ActionResult Detail(int id, int type = 0)
        {
            ProductModel model;
            List<CompanyModel> comps;
            var typeList = new List<CategoryModel>();

            // 获取分类下拉框数据
            using (var categoryService = ResolveService<CategoryService>())
            {
                var rs = categoryService.GetByTarget(target: CategoryTarget.产品分类.ToString());
                if (rs.Status && rs.Data != null)
                {
                    typeList = rs.Data as List<CategoryModel>;
                }
            }

            using (var productService = ResolveService<ProductService>())
            {
                var rs = productService.GetById(id);
                if (rs.Status && rs.Data != null)
                {
                    var entity = rs.Data as Product;
                    model = new ProductModel
                    {
                        Id = entity.Id,
                        Type = entity.Type,
                        TypeName = entity.TypeName,
                        Name = entity.Name,

                        Level = entity.Level,
                        State = entity.State,
                        Application = entity.Application,
                        Company = entity.Company,
                        Description = entity.Description,

                        SaveAt = DateTime.Now,
                        ImgSrc = entity.ImgSrc
                    };
                }
                else
                {
                    model = new ProductModel();
                }
            }

            using (var companyService = ResolveService<CompanyService>())
            {
                // 上下游企业
                var rs = companyService.GetList("select top 20 Id,Name from [Company] order by Type asc");
                if (rs.Status && rs.Data != null)
                {
                    comps = rs.Data as List<CompanyModel>;
                }
                else
                {
                    comps = new List<CompanyModel>();
                }
            }

            var briefs = new List<BriefModel>(); // 3条简报

            using (var briefService = ResolveService<BriefService>())
            {
                var rs = briefService.GetList("select top 3 Id,Name,FilePath from [Brief] order by SaveAt desc");
                if (rs.Status && rs.Data != null)
                {
                    briefs = rs.Data as List<BriefModel>;
                }
            }

            ViewBag.Briefs = briefs;
            ViewBag.Type = type;
            ViewBag.TypeList = typeList;
            ViewBag.Product = model;
            ViewBag.Companies = comps;
            return View("Detail");
        }

        #endregion

    }
}
