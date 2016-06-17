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
    public class CompanyController : BaseController
    {
        [HttpGet]
        [Authentication]
        public ActionResult Company(int id = 0)
        {
            var company = new CompanyModel();
            var catList = new List<CategoryModel>();

            // Id > 0 是编辑；Id = 0 是新建
            if (id > 0)
            {
                using (var companyService = ResolveService<CompanyService>())
                {
                    var rs = companyService.GetById(id);
                    if (rs.Status && rs.Data != null)
                    {
                        var entity = rs.Data as Company;
                        company = new CompanyModel
                        {
                            Id = entity.Id,
                            Name = entity.Name,
                            Type = entity.Type,
                            Site = entity.Site,
                            LogoSrc = entity.LogoSrc,
                            Description = entity.Description,
                            SaveAt = DateTime.Now,
                            Range = entity.Range,
                            RangeName = entity.RangeName,
                            Index = entity.Index,
                            CIndex = entity.CIndex,
                        };
                    }
                }
            }

            // 获取分类下拉框数据
            using (var categoryService = ResolveService<CategoryService>())
            {
                var rs = categoryService.GetByTarget(target: CategoryTarget.成员企业分类.ToString());
                if (rs.Status && rs.Data != null)
                {
                    catList = rs.Data as List<CategoryModel>;
                }
            }

            ViewBag.Company = company;
            ViewBag.CatList = catList;
            ViewBag.Nav = "CompanyList";
            return View();
        }

        [HttpPost]
        [Authentication]
        public JsonResult Save(CompanyModel model)
        {
            // 若无，则取出后放入session
            using (var companyService = ResolveService<CompanyService>())
            {
                // Id > 0 修改，Id = 0 新增
                JsonRespModel respModel;
                if (model.Id > 0)
                {
                    var svs = companyService.Update(new Company
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Type = model.Type,
                        Site = !string.IsNullOrEmpty(model.Site) && !model.Site.Trim().Contains("http:") && !model.Site.Trim().Contains("https:") ? string.Concat("http://", model.Site.Trim()) : model.Site,
                        LogoSrc = model.LogoSrc,
                        Description = model.Description,
                        SaveAt = DateTime.Now,
                        Range = model.Range,
                        RangeName = model.RangeName,
                        Index = model.Index,
                        CIndex = model.CIndex,
                    });
                    respModel = new JsonRespModel { status = svs.Status, message = svs.Status ? "修改成功！" : "修改失败！" };
                    companyService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", svs.Status ? "修改成功！" : "修改失败！", "成员修改页") });
                }
                else
                {
                    var svs = companyService.Insert(new Company
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Type = model.Type,
                        Site = !string.IsNullOrEmpty(model.Site) && !model.Site.Trim().Contains("http:") && !model.Site.Trim().Contains("https:") ? string.Concat("http://", model.Site.Trim()) : model.Site,
                        LogoSrc = model.LogoSrc,
                        Description = model.Description,
                        SaveAt = DateTime.Now,
                        Range = model.Range,
                        RangeName = model.RangeName,
                        Index = model.Index,
                        CIndex = model.CIndex,
                    });
                    respModel = new JsonRespModel { status = svs.Status, message = svs.Status ? "新建成功！" : "新建失败！" };
                    companyService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", svs.Status ? "新建成功！" : "新建失败！", "成员新建页") });
                }

                return Json(respModel);
            }
        }

        [HttpGet]
        [Authentication]
        public ActionResult Delete(string ids, string type, int range, string keys)
        {
            using (var companyService = ResolveService<CompanyService>())
            {
                var rs = companyService.Delete(ids.Split(',').Select(int.Parse).ToArray());
                companyService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", rs.Status ? "删除成功！" : "删除失败！", "成员删除页") });
                return RedirectToAction("List", new { type = type, range = range, keys = keys });
            }
        }

        [HttpGet]
        [Authentication]
        public ActionResult List(string type, string keys, int range = 0, int pindex = 1, int psize = 2)
        {
            List<CompanyModel> models;
            var catList = new List<CategoryModel>();
            int itemCount;

            using (var companyService = ResolveService<CompanyService>())
            {
                // 按条件查询 sql
                var where = " where 1=1 ";
                if (!string.IsNullOrEmpty(type))
                {
                    where += string.Format(" and a.Type = '{0}' ", type);
                }
                if (range > 0)
                {
                    where += string.Format(" and a.Range = {0} ", range);
                }
                if (!string.IsNullOrEmpty(keys))
                {
                    var keyArr = keys.Split(',', '，', ' ');
                    where += " and ( 1=2 ";
                    where = keyArr.Aggregate(where, (current, key) => current + string.Format(" or CHARINDEX('{0}',a.Name)>0 ", key.Trim()));
                    where += ")";
                }

                psize = AppSettingHelper.GetInt("PageSize") != 0 ? AppSettingHelper.GetInt("PageSize") : psize;
                var query = string.Format(@"(select a.*,b.[Index] RangeIndex  from [Company] a inner join [Category] b on a.Range = b.Id {0})", where);
                var rs = companyService.GetByPage(query, "RangeIndex desc, [CIndex] desc, [Index] desc", pindex, psize, out itemCount);
                models = rs.Status ? rs.Data as List<CompanyModel> : new List<CompanyModel>();
                companyService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", rs.Status ? "查询成功！" : "查询失败！", "成员列表页") });
            }

            // 获取分类下拉框数据
            using (var categoryService = ResolveService<CategoryService>())
            {
                var rs = categoryService.GetByTarget(target: CategoryTarget.成员企业分类.ToString());
                if (rs.Status && rs.Data != null)
                {
                    catList = rs.Data as List<CategoryModel>;
                }
            }

            ViewBag.Keys = keys;
            ViewBag.Range = range; // 可增减类型
            ViewBag.Type = type; // 固定类型
            ViewBag.Companies = models;
            ViewBag.CatList = catList;
            ViewBag.PageInfo = new PageInfo(pindex, psize, itemCount, (itemCount % psize == 0) ? (itemCount / psize) : (itemCount / psize + 1));
            ViewBag.Nav = "CompanyList";
            return View();
        }

    }
}
