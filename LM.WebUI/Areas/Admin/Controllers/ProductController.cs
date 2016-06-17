using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LM.Model.Common;
using LM.Model.Entity;
using LM.Model.Model;
using LM.Service;
using LM.Service.Base;
using LM.Service.Security;
using LM.Utility;
using LM.WebUI.Areas.Admin.Models;

namespace LM.WebUI.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        [HttpGet]
        [Authentication]
        public ActionResult Product(int id = 0)
        {
            var model = new ProductModel();
            var catList = new List<CategoryModel>();

            // Id > 0 是编辑；Id = 0 是新建
            if (id > 0)
            {
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
                }
            }

            // 获取分类下拉框数据
            using (var categoryService = ResolveService<CategoryService>())
            {
                var rs = categoryService.GetByTarget(target: CategoryTarget.产品分类.ToString());
                if (rs.Status && rs.Data != null)
                {
                    catList = rs.Data as List<CategoryModel>;
                }
            }

            ViewBag.Product = model;
            ViewBag.CatList = catList;
            ViewBag.Nav = "ProductList";
            return View();
        }


        [HttpGet]
        [Authentication]
        public ActionResult List(string keys, int type = 0, int pindex = 1, int psize = 2)
        {
            List<ProductModel> models;
            var catList = new List<CategoryModel>();
            int itemCount;

            using (var productService = ResolveService<ProductService>())
            {
                // 按条件查询 sql
                var where = " where 1=1 ";
                if (type > 0)
                {
                    where += string.Format(" and a.Type = {0} ", type);
                }
                if (!string.IsNullOrEmpty(keys))
                {
                    var keyArr = keys.Split(',', '，', ' ');
                    where += " and ( 1=2 ";
                    where = keyArr.Aggregate(where, (current, key) => current + string.Format(" or CHARINDEX('{0}',a.Name)>0 ", key));
                    where += ")";
                }

                psize = AppSettingHelper.GetInt("PageSize") != 0 ? AppSettingHelper.GetInt("PageSize") : psize;
                var query = string.Format(@"(select a.* from [Product] a {0})", where);
                var rs = productService.GetByPage(query, "SaveAt desc", pindex, psize, out itemCount);
                models = rs.Status ? rs.Data as List<ProductModel> : new List<ProductModel>();

                productService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", rs.Status ? "查询成功！" : "查询失败！", "产品列表页") });
            }

            // 获取分类下拉框数据
            using (var categoryService = ResolveService<CategoryService>())
            {
                var rs = categoryService.GetByTarget(target: CategoryTarget.产品分类.ToString());
                if (rs.Status && rs.Data != null)
                {
                    catList = rs.Data as List<CategoryModel>;
                }
            }

            ViewBag.Keys = keys;
            ViewBag.Type = type;
            ViewBag.Products = models;
            ViewBag.CatList = catList;
            ViewBag.PageInfo = new PageInfo(pindex, psize, itemCount, (itemCount % psize == 0) ? (itemCount / psize) : (itemCount / psize + 1));
            ViewBag.Nav = "ProductList";
            return View();
        }


        [HttpPost]
        [Authentication]
        public JsonResult Save(ProductModel model)
        {
            // 若无，则取出后放入session
            using (var productService = ResolveService<ProductService>())
            {
                // Id > 0 修改，Id = 0 新增
                JsonRespModel respModel;
                if (model.Id > 0)
                {
                    var svs = productService.Update(new Product
                    {
                        Id = model.Id,
                        Type = model.Type,
                        TypeName = model.TypeName,
                        Name = model.Name,

                        Level = model.Level,
                        State = model.State,
                        Application = model.Application,
                        Company = model.Company,
                        Description = model.Description.StartsWith("<") ? model.Description : string.Format("{0}{1}{2}", "<p>", model.Description, "</p>"),

                        SaveAt = DateTime.Now,
                        ImgSrc = model.ImgSrc
                    });
                    respModel = new JsonRespModel { status = svs.Status, message = svs.Status ? "修改成功！" : "修改失败！" };
                    productService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", svs.Status ? "修改成功！" : "修改失败！", "产品修改页") });
                }
                else
                {
                    var svs = productService.Insert(new Product
                    {
                        Id = model.Id,
                        Type = model.Type,
                        TypeName = model.TypeName,
                        Name = model.Name,

                        Level = model.Level,
                        State = model.State,
                        Application = model.Application,
                        Company = model.Company,
                        Description = model.Description.StartsWith("<") ? model.Description : string.Format("{0}{1}{2}", "<p>", model.Description, "</p>"),

                        SaveAt = DateTime.Now,
                        ImgSrc = model.ImgSrc
                    });
                    respModel = new JsonRespModel { status = svs.Status, message = svs.Status ? "新建成功！" : "新建失败！" };
                    productService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", svs.Status ? "新建成功！" : "新建失败！", "产品新建页") });
                }

                return Json(respModel);
            }
        }


        [HttpGet]
        [Authentication]
        public ActionResult Delete(string ids, int type, string keys)
        {
            using (var productService = ResolveService<ProductService>())
            {
                var rs = productService.Delete(ids.Split(',').Select(int.Parse).ToArray());
                productService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", rs.Status ? "删除成功！" : "删除失败！", "产品删除页") });
                return RedirectToAction("List", new { type = type, keys = keys });
            }
        }
    }
}
