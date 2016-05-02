using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    public class CategoryController : BaseController
    {
        public ActionResult Index()
        {
            return View("List");
        }

        [HttpGet]
        [Authentication]
        public ActionResult Category(int id = 0)
        {
            var category = new CategoryModel();

            // Id > 0 是编辑；Id = 0 是新建
            if (id > 0)
            {
                using (var categoryService = ResolveService<CategoryService>())
                {
                    var rs = categoryService.GetById(id);
                    if (rs.Status && rs.Data != null)
                    {
                        var entity = rs.Data as Category;
                        category = new CategoryModel
                        {
                            Id = entity.Id,
                            Name = entity.Name,
                            Target = entity.Target,
                            Description = entity.Description,
                            Index = entity.Index,
                            SaveAt = DateTime.Now,
                        };
                    }
                }
            }

            ViewBag.Category = category;
            ViewBag.Nav = "CategoryList";
            return View();
        }

        [HttpPost]
        [Authentication]
        public JsonResult Save(CategoryModel model)
        {
            // 若无，则取出后放入session
            using (var categoryService = ResolveService<CategoryService>())
            {
                // Id > 0 修改，Id = 0 新增
                JsonRespModel respModel;
                if (model.Id > 0)
                {
                    var svs = categoryService.Update(new Category
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Target = model.Target,
                        Description = model.Description,
                        Index = model.Index,
                        SaveAt = DateTime.Now,
                    });
                    respModel = new JsonRespModel { status = svs.Status, message = svs.Status ? "修改成功！" : "修改失败！" };
                    categoryService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", svs.Status ? "修改成功！" : "修改失败！", "分类修改页") });
                }
                else
                {
                    var svs = categoryService.Insert(new Category
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Target = model.Target,
                        Description = model.Description,
                        Index = model.Index,
                        SaveAt = DateTime.Now,
                    });
                    respModel = new JsonRespModel { status = svs.Status, message = svs.Status ? "新建成功！" : "新建失败！" };
                    categoryService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", svs.Status ? "新建成功！" : "新建失败！", "分类新建页") });
                }

                return Json(respModel);
            }
        }

        [HttpGet]
        [Authentication]
        public ActionResult Delete(string ids, string target, string keys)
        {
            using (var categoryService = ResolveService<CategoryService>())
            {
                var rs = categoryService.Delete(ids.Split(',').Select(int.Parse).ToArray());
                categoryService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", rs.Status ? "删除成功！" : "删除失败！", "分类删除页") });
                return RedirectToAction("List", new { target = target, keys = keys });
            }
        }

        [HttpGet]
        [Authentication]
        public ActionResult List(string target, string keys, int pindex = 1, int psize = 2)
        {
            List<CategoryModel> models;
            int itemCount;

            using (var categoryService = ResolveService<CategoryService>())
            {
                // 按条件查询 sql
                var where = " where 1=1 ";
                if (!string.IsNullOrEmpty(target))
                {
                    where += string.Format(" and a.Target = '{0}' ", target);
                }
                if (!string.IsNullOrEmpty(keys))
                {
                    var keyArr = keys.Split(',', '，', ' ');
                    where += " and ( 1=2 ";
                    where = keyArr.Aggregate(where, (current, key) => current + string.Format(" or CHARINDEX('{0}',a.Name)>0 ", key));
                    where += ")";
                }

                psize = AppSettingHelper.GetInt("PageSize") != 0 ? AppSettingHelper.GetInt("PageSize") : psize;
                var query = string.Format(@"(select a.* from [Category] a {0})", where);
                var rs = categoryService.GetByPage(query, "[Target] asc, [Index] desc", pindex, psize, out itemCount);
                models = rs.Status ? rs.Data as List<CategoryModel> : new List<CategoryModel>();
                categoryService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", rs.Status ? "查询成功！" : "查询失败！", "分类列表页") });
            }

            ViewBag.Keys = keys;
            ViewBag.Target = target;
            ViewBag.Categories = models;
            ViewBag.PageInfo = new PageInfo(pindex, psize, itemCount, (itemCount % psize == 0) ? (itemCount / psize) : (itemCount / psize + 1));
            ViewBag.Nav = "CategoryList";
            return View();
        }
    }
}
