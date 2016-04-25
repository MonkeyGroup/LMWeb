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
    public class ExpertController : BaseController
    {
        [Authentication]
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        [Authentication]
        public ActionResult Expert(int id = 0)
        {
            var model = new ExpertModel();

            // Id > 0 是编辑；Id = 0 是新建
            if (id > 0)
            {
                using (var expertService = ResolveService<ExpertService>())
                {
                    var rs = expertService.GetById(id);
                    if (rs.Status && rs.Data != null)
                    {
                        var entity = rs.Data as Expert;
                        model = new ExpertModel
                        {
                            Id = entity.Id,
                            Name = entity.Name,
                            ImgSrc = entity.ImgSrc,
                            Range = entity.Range,
                            Description = entity.Description,
                            Books = entity.Books,
                            SaveAt = DateTime.Now
                        };
                    }
                }
            }

            ViewBag.Expert = model;
            ViewBag.Nav = "ExpertList";
            return View();
        }

        [HttpGet]
        [Authentication]
        public ActionResult List(string type, string keys, int pindex = 1, int psize = 2)
        {
            List<ExpertModel> models;
            int itemCount;

            using (var expertService = ResolveService<ExpertService>())
            {
                // 按条件查询 sql
                var where = " where 1=1 ";
                if (!string.IsNullOrEmpty(type))
                {
                    where += string.Format(" and a.Range = '{0}' ", type);
                }
                if (!string.IsNullOrEmpty(keys))
                {
                    var keyArr = keys.Split(',', '，', ' ');
                    where += " and ( 1=2 ";
                    where = keyArr.Aggregate(where, (current, key) => current + string.Format(" or CHARINDEX('{0}',a.Name)>0 ", key));
                    where += ")";
                }

                psize = AppSettingHelper.GetInt("PageSize") != 0 ? AppSettingHelper.GetInt("PageSize") : pindex;
                var query = string.Format(@"(select a.* from [Expert] a {0})", where);
                var rs = expertService.GetByPage(query, "SaveAt desc", pindex, psize, out itemCount);
                models = rs.Status ? rs.Data as List<ExpertModel> : new List<ExpertModel>();

                expertService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", rs.Status ? "查询成功！" : "查询失败！", "专家列表页") });
            }

            ViewBag.Keys = keys;
            ViewBag.Type = type;
            ViewBag.Experts = models;
            ViewBag.PageInfo = new PageInfo(pindex, psize, itemCount, (itemCount % psize == 0) ? (itemCount / psize) : (itemCount / psize + 1));
            ViewBag.Nav = "ExpertList";
            return View();
        }


        [HttpPost]
        [Authentication]
        public JsonResult Save(ExpertModel model)
        {
            // 若无，则取出后放入session
            using (var expertService = ResolveService<ExpertService>())
            {
                // Id > 0 修改，Id = 0 新增
                JsonRespModel respModel;
                if (model.Id > 0)
                {
                    var svs = expertService.Update(new Expert
                    {
                        Id = model.Id,
                        Name = model.Name,
                        ImgSrc = model.ImgSrc,
                        Range = model.Range,
                        Description = model.Description,
                        Books = model.Books,
                        SaveAt = DateTime.Now
                    });
                    respModel = new JsonRespModel { status = svs.Status, message = svs.Status ? "修改成功！" : "修改失败！" };
                    expertService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", svs.Status ? "修改成功！" : "修改失败！", "专家修改页") });
                }
                else
                {
                    var svs = expertService.Insert(new Expert
                    {
                        Id = model.Id,
                        Name = model.Name,
                        ImgSrc = model.ImgSrc,
                        Range = model.Range,
                        Description = model.Description,
                        Books = model.Books,
                        SaveAt = DateTime.Now
                    });
                    respModel = new JsonRespModel { status = svs.Status, message = svs.Status ? "新建成功！" : "新建失败！" };
                    expertService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", svs.Status ? "新建成功！" : "新建失败！", "专家新建页") });
                }

                return Json(respModel);
            }
        }

        [HttpGet]
        [Authentication]
        public ActionResult Delete(string ids, string type, string keys)
        {
            using (var expertService = ResolveService<ExpertService>())
            {
                var rs = expertService.Delete(ids.Split(',').Select(int.Parse).ToArray());
                expertService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", rs.Status ? "删除成功！" : "删除失败！", "专家删除页") });
                return RedirectToAction("List", new { type = type, keys = keys });
            }
        }
    }
}
