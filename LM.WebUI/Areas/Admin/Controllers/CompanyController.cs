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
    public class CompanyController : BaseController
    {
        [Authentication]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authentication]
        public ActionResult Company(int id = 0)
        {
            var company = new CompanyModel();

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
                            Range = entity.Range
                        };
                    }
                }
            }

            ViewBag.Company = company;
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
                        Site = model.Site,
                        LogoSrc = model.LogoSrc,
                        Description = model.Description,
                        SaveAt = DateTime.Now,
                        Range = model.Range
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
                        Site = model.Site,
                        LogoSrc = model.LogoSrc,
                        Description = model.Description,
                        SaveAt = DateTime.Now,
                        Range = model.Range
                    });
                    respModel = new JsonRespModel { status = svs.Status, message = svs.Status ? "新建成功！" : "新建失败！" };
                    companyService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", svs.Status ? "新建成功！" : "新建失败！", "成员新建页") });
                }

                return Json(respModel);
            }
        }
        
        [HttpGet]
        [Authentication]
        public ActionResult Delete(string ids, string type, string range, string keys)
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
        public ActionResult List(string type, string range, string keys, int pindex = 1, int psize = 2)
        {
            List<CompanyModel> models;
            int itemCount;

            using (var companyService = ResolveService<CompanyService>())
            {
                // 按条件查询 sql
                var where = " where 1=1 ";
                if (!string.IsNullOrEmpty(type))
                {
                    where += string.Format(" and a.Type = '{0}' ", type);
                }
                if (!string.IsNullOrEmpty(range))
                {
                    where += string.Format(" and a.Range = '{0}' ", range);
                }
                if (!string.IsNullOrEmpty(keys))
                {
                    var keyArr = keys.Split(',', '，', ' ');
                    where += " and ( 1=2 ";
                    where = keyArr.Aggregate(where, (current, key) => current + string.Format(" or CHARINDEX('{0}',a.Name)>0 ", key));
                    where += ")";
                }

                psize = AppSettingHelper.GetInt("PageSize") != 0 ? AppSettingHelper.GetInt("PageSize") : psize;
                var query = string.Format(@"(select a.* from [Company] a {0})", where);
                var rs = companyService.GetByPage(query, "SaveAt desc", pindex, psize, out itemCount);
                models = rs.Status ? rs.Data as List<CompanyModel> : new List<CompanyModel>();
                companyService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", rs.Status ? "查询成功！" : "查询失败！", "成员列表页") });
            }

            ViewBag.Keys = keys;
            ViewBag.Range = range;
            ViewBag.Type = type;
            ViewBag.Companies = models;
            ViewBag.PageInfo = new PageInfo(pindex, psize, itemCount, (itemCount % psize == 0) ? (itemCount / psize) : (itemCount / psize + 1));
            ViewBag.Nav = "CompanyList";
            return View();
        }

    }
}
