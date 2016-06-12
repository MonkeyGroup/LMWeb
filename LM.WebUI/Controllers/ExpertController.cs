using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LM.Model.Entity;
using LM.Model.Model;
using LM.Service;
using LM.Service.Base;
using LM.WebUI.Areas.Admin.Models;

namespace LM.WebUI.Controllers
{
    public class ExpertController : BaseController
    {

        #region 专家委员会

        /// <summary>
        ///  已废弃！
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pindex"></param>
        /// <returns></returns>
        public ActionResult List1(int id = 0, int pindex = 1)
        {
            var currExpert = new ExpertModel();
            var models = new List<ExpertModel>();
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
            ViewBag.PageInfo = new PageInfo(pindex, psize, itemCount, (itemCount % psize == 0) ? (itemCount / psize) : (itemCount / psize + 1));
            return View();
        }

        public ActionResult List()
        {
            var models = new List<ExpertModel>();

            // 获取列表数据
            using (var expertService = ResolveService<ExpertService>())
            {
                var rs = expertService.GetList("select * from [Expert] order by Range asc,Id asc");
                if (rs.Status && rs.Data != null)
                {
                    models = rs.Data as List<ExpertModel>;
                }
            }

            var slideArticles = new List<ArticleModel>(); // 幻灯片数据
            var dynamicArticles = new List<ArticleModel>(); // 联盟动态

            using (var articleService = ResolveService<ArticleService>())
            {
                // 幻灯片数据
                var rs1 = articleService.GetList("select top 4 Id,Type,Title,ImgSrc from [Article] where IsShow = 1 and ImgSrc is not null order by SaveAt desc, Id desc");
                if (rs1.Status && rs1.Data != null)
                {
                    slideArticles = rs1.Data as List<ArticleModel>;
                }
                // 联盟动态 
                var rs2 = articleService.GetList("select top 10 Id,Title,SaveAt from [Article] where Type = '联盟动态' order by  SaveAt desc,Id desc");
                if (rs2.Status && rs2.Data != null)
                {
                    dynamicArticles = rs2.Data as List<ArticleModel>;
                }
            }

            ViewBag.SlideArticles = slideArticles;
            ViewBag.DynamicArticles = dynamicArticles;
            ViewBag.Experts = models;
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
