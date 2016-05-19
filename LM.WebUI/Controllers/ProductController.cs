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
                psize = 6;
                type = type == 0 && typeList.Count > 0 ? typeList.First().Id : type;
                var query = string.Format(@"(select a.Id, a.Name, a.ImgSrc,a.Level,a.State, a.Application, a.SaveAt from [Product] a Where a.Type = {0})", type);
                var orderby = "SaveAt desc";
                var rs = productService.GetByPage(query, orderby, pindex, psize, out itemCount);
                if (rs.Status && rs.Data != null)
                {
                    models = rs.Data as List<ProductModel>;
                }
            }

            ViewBag.Type = type;
            ViewBag.Products = models;
            ViewBag.TypeList = typeList;
            ViewBag.PageInfo = new PageInfo(pindex, psize, itemCount, (itemCount % psize == 0) ? (itemCount / psize) : (itemCount / psize + 1));
            return View("List2");
        }

        public ActionResult Detail(int id)
        {
            ProductModel model;
            List<CompanyModel> comps;

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

                        //Company1Name = entity.Company1Name,
                        //Company1Phone = entity.Company1Phone,
                        //Company1Fax = entity.Company1Fax,
                        //Company1Email = entity.Company1Email,
                        //Company1Linkman = entity.Company1Linkman,

                        //Company2Name = entity.Company2Name,
                        //Company2Phone = entity.Company2Phone,
                        //Company2Fax = entity.Company2Fax,
                        //Company2Email = entity.Company2Email,
                        //Company2Linkman = entity.Company2Linkman,

                        //Company3Name = entity.Company3Name,
                        //Company3Phone = entity.Company3Phone,
                        //Company3Fax = entity.Company3Fax,
                        //Company3Email = entity.Company3Email,
                        //Company3Linkman = entity.Company3Linkman,

                        //Description1 = entity.Description1,
                        //Description2 = entity.Description2,
                        //Description3 = entity.Description3,
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

            ViewBag.Product = model;
            ViewBag.Companies = comps;
            return View("Detail2");
        }

        #endregion

    }
}
