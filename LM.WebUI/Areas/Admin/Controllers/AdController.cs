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
    public class AdController : BaseController
    {
        [HttpGet]
        [Authentication]
        public ActionResult List(int pindex = 1, int psize = 2)
        {
            List<AdModel> models;
            int itemCount;

            using (var adService = ResolveService<AdService>())
            {
                psize = AppSettingHelper.GetInt("PageSize") != 0 ? AppSettingHelper.GetInt("PageSize") : psize;
                var rs = adService.GetByPage("[Ad]", "[Index] desc, Id desc", pindex, psize, out itemCount);
                models = rs.Status ? rs.Data as List<AdModel> : new List<AdModel>();

                adService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", rs.Status ? "查询成功！" : "查询失败！", "广告列表页") });
            }

            ViewBag.Ads = models;
            ViewBag.PageInfo = new PageInfo(pindex, psize, itemCount, (itemCount % psize == 0) ? (itemCount / psize) : (itemCount / psize + 1));
            ViewBag.Nav = "AdList";
            return View();
        }

        [HttpGet]
        [Authentication]
        public ActionResult Ad(int id = 0)
        {
            var model = new AdModel();

            // Id > 0 是编辑；Id = 0 是新建
            if (id > 0)
            {
                using (var adService = ResolveService<AdService>())
                {
                    var rs = adService.GetById(id);
                    if (rs.Status && rs.Data != null)
                    {
                        var entity = rs.Data as Ad;
                        model = new AdModel
                        {
                            Id = entity.Id,
                            Name = entity.Name,
                            LinkUrl = entity.LinkUrl,
                            ImgSrc = entity.ImgSrc,
                            Index = entity.Index,
                            SaveAt = entity.SaveAt,
                        };
                    }
                }
            }

            ViewBag.Ad = model;
            ViewBag.Nav = "AdList";
            return View();
        }

        [HttpPost]
        [Authentication]
        public JsonResult Save(AdModel model)
        {
            // 若无，则取出后放入session
            using (var adService = ResolveService<AdService>())
            {
                // Id > 0 修改，Id = 0 新增
                JsonRespModel respModel;
                if (model.Id > 0)
                {
                    var svs = adService.Update(new
                    {
                        model.Id,
                        model.Name,
                        model.ImgSrc,
                        LinkUrl = !string.IsNullOrEmpty(model.LinkUrl) && !model.LinkUrl.Contains("http:") && !model.LinkUrl.Contains("https:") ? string.Concat("http://", model.LinkUrl) : model.LinkUrl,
                        model.Index,
                        SaveAt = DateTime.Now
                    });
                    respModel = new JsonRespModel { status = svs.Status, message = svs.Status ? "修改成功！" : "修改失败！" };
                    adService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", svs.Status ? "修改成功！" : "修改失败！", "广告修改页") });
                }
                else
                {
                    var svs = adService.Insert(new Ad
                    {
                        Id = model.Id,
                        Name = model.Name,
                        LinkUrl = !string.IsNullOrEmpty(model.LinkUrl) && !model.LinkUrl.Contains("http:") && !model.LinkUrl.Contains("https:") ? string.Concat("http://", model.LinkUrl) : model.LinkUrl,
                        ImgSrc = model.ImgSrc,
                        Index = model.Index,
                        SaveAt = DateTime.Now
                    });
                    respModel = new JsonRespModel { status = svs.Status, message = svs.Status ? "新建成功！" : "新建失败！" };
                    adService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", svs.Status ? "新建成功！" : "新建失败！", "广告新建页") });
                }

                return Json(respModel);
            }
        }

        [HttpGet]
        [Authentication]
        public ActionResult Delete(string ids)
        {
            using (var adService = ResolveService<AdService>())
            {
                var rs = adService.Delete(ids.Split(',').Select(int.Parse).ToArray());
                adService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", rs.Status ? "删除成功！" : "删除失败！", "广告删除页") });
                return RedirectToAction("List");
            }
        }

    }
}
