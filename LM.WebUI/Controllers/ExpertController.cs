using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LM.Model.Common;
using LM.Model.Entity;
using LM.Model.Model;
using LM.Service;
using LM.Service.Base;
using LM.WebUI.Areas.Admin.Models;

namespace LM.WebUI.Controllers
{
    public class ExpertController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }


        #region 专家委员会

        public ActionResult List(int id = 0, int pindex = 1)
        {
            var currExpert = new ExpertModel();
            var models = new List<ExpertModel>();
            var typeList = new Dictionary<int, string>
            {
                {(int)ExpertRange.专家组组长,ExpertRange.专家组组长.ToString()},
                {(int)ExpertRange.专家组副组长,ExpertRange.专家组副组长.ToString()},
                {(int)ExpertRange.专家组成员,ExpertRange.专家组成员.ToString()},
            };
            int itemCount, psize;

            // 获取列表数据
            using (var expertService = ResolveService<ExpertService>())
            {
                psize = 8;
                var orderby = "Range asc,SaveAt desc";
                var rs = expertService.GetByPage("[Expert]", orderby, pindex, psize, out itemCount);
                if (rs.Status && rs.Data != null)
                {
                    models = rs.Data as List<ExpertModel>;
                }
            }

            // 获取当前
            if (id > 0)
            {
                using (var expertService = ResolveService<ExpertService>())
                {
                    var rs = expertService.GetById(id);
                    if (rs.Status && rs.Data != null)
                    {
                        var entity = rs.Data as Expert;
                        currExpert = new ExpertModel
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
            else
            {
                currExpert = models.FirstOrDefault();
            }

            ViewBag.DefExpert = currExpert;
            ViewBag.Experts = models;
            ViewBag.TypeList = typeList;
            ViewBag.PageInfo = new PageInfo(pindex, psize, itemCount, (itemCount % psize == 0) ? (itemCount / psize) : (itemCount / psize + 1));
            return View();
        }

        public JsonResult Detail(int id)
        {
            var model = new ExpertModel();
            var resp = new JsonRespModel();

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
                resp = new JsonRespModel { data = model, status = rs.Status };
            }

            return Json(resp);
        }

        #endregion

    }
}
