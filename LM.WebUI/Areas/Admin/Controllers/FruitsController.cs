using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
    public class FruitsController : BaseController
    {
        [HttpGet]
        [Authentication]
        public ActionResult Index()
        {
            return View("List");
        }

        [HttpGet]
        [Authentication]
        public ActionResult List(string keys, int pindex = 1, int psize = 2)
        {
            List<FruitsModel> models;
            int itemCount;

            using (var fruitsService = ResolveService<FruitsService>())
            {
                // 按条件查询 sql
                var where = " where 1=1 ";
                if (!string.IsNullOrEmpty(keys))
                {
                    var keyArr = keys.Split(',', '，', ' ');
                    where += " and ( 1=2 ";
                    where = keyArr.Aggregate(where, (current, key) => current + string.Format(" or CHARINDEX('{0}',a.Name)>0 ", key.Trim()));
                    where += ")";
                }

                psize = AppSettingHelper.GetInt("PageSize") != 0 ? AppSettingHelper.GetInt("PageSize") : psize;
                var query = string.Format(@"(select a.* from [Fruits] a {0})", where);
                var rs = fruitsService.GetByPage(query, "SaveAt desc", pindex, psize, out itemCount);
                models = rs.Status ? rs.Data as List<FruitsModel> : new List<FruitsModel>();

                fruitsService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", rs.Status ? "查询成功！" : "查询失败！", "文章列表页") });
            }

            ViewBag.Keys = keys;
            ViewBag.Fruits = models;
            ViewBag.PageInfo = new PageInfo(pindex, psize, itemCount, (itemCount % psize == 0) ? (itemCount / psize) : (itemCount / psize + 1));
            ViewBag.Nav = "FruitsList";
            return View();
        }

        [HttpGet]
        [Authentication]
        public ActionResult Fruits(int id = 0)
        {
            var model = new FruitsModel();

            // Id > 0 是编辑；Id = 0 是新建
            if (id > 0)
            {
                using (var fruitsService = ResolveService<FruitsService>())
                {
                    var rs = fruitsService.GetById(id);
                    if (rs.Status && rs.Data != null)
                    {
                        var entity = rs.Data as Fruits;
                        model = new FruitsModel
                        {
                            Id = entity.Id,
                            Name = entity.Name,
                            Type = entity.Type,
                            Company = entity.Company,
                            Leader = entity.Leader,
                            Patent = entity.Patent,
                            Rights = entity.Rights,
                            Awards = entity.Awards,
                            Application = entity.Application,
                            Description = entity.Description,
                            SaveAt = entity.SaveAt
                        };
                    }
                }
            }

            ViewBag.Fruits = model;
            ViewBag.Nav = "FruitsList";
            return View();
        }

        [HttpPost]
        [Authentication]
        public JsonResult Save(FruitsModel model)
        {
            // 若无，则取出后放入session
            using (var fruitsService = ResolveService<FruitsService>())
            {
                // Id > 0 修改，Id = 0 新增
                JsonRespModel respModel;
                if (model.Id > 0)
                {
                    var svs = fruitsService.Update(new Fruits
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Type = model.Type,
                        Company = model.Company,
                        Leader = model.Leader,
                        Patent = model.Patent,
                        Awards = model.Awards,
                        Application = model.Application,
                        Description = model.Description,
                        SaveAt = model.SaveAt.ToString("yyyy-MM-dd").Equals("0001-01-01") ? DateTime.Now : model.SaveAt
                    });
                    respModel = new JsonRespModel { status = svs.Status, message = svs.Status ? "修改成功！" : "修改失败！" };
                    fruitsService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", svs.Status ? "修改成功！" : "修改失败！", "文章修改页") });
                }
                else
                {
                    var svs = fruitsService.Insert(new Fruits
                    {
                        Name = model.Name,
                        Type = model.Type,
                        Company = model.Company,
                        Leader = model.Leader,
                        Patent = model.Patent,
                        Awards = model.Awards,
                        Application = model.Application,
                        Description = model.Description.StartsWith("<p>") ? model.Description : string.Format("{0}{1}{2}", "<p>", model.Description, "</p>"),
                        SaveAt = model.SaveAt.ToString("yyyy-MM-dd").Equals("0001-01-01") ? DateTime.Now : model.SaveAt
                    });
                    respModel = new JsonRespModel { status = svs.Status, message = svs.Status ? "新建成功！" : "新建失败！" };
                    fruitsService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", svs.Status ? "新建成功！" : "新建失败！", "文章新建页") });
                }

                return Json(respModel);
            }
        }

        [Authentication]
        public ActionResult Delete(string ids, string type, string keys)
        {
            using (var fruitsService = ResolveService<FruitsService>())
            {
                var rs = fruitsService.Delete(ids.Split(',').Select(int.Parse).ToArray());
                fruitsService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", rs.Status ? "删除成功！" : "删除失败！", "文章删除页") });
                return RedirectToAction("List", new { type = type, keys = keys });
            }
        }

    }
}
