using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LM.Model.Entity;
using LM.Model.Model;
using LM.Service;
using LM.WebUI.Areas.Admin.Models;

namespace LM.WebUI.Controllers
{
    public class FruitsController : BaseController
    {
        /// <summary>
        ///  科研成果列表
        /// </summary>
        /// <param name="pindex"></param>
        /// <returns></returns>
        public ActionResult List(int pindex = 1)
        {
            var models = new List<FruitsModel>();
            int itemCount, psize;

            using (var fruitsService = ResolveService<FruitsService>())
            {
                psize = 15;
                var query = string.Format(@"(select Id,Name,Type,Company,Leader,Patent,Rights,Awards,Application from [Fruits])");
                var orderby = "Id desc";
                var rs = fruitsService.GetByPage(query, orderby, pindex, psize, out itemCount);
                models = rs.Status ? rs.Data as List<FruitsModel> : models;
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
            ViewBag.Models = models;
            ViewBag.PageInfo = new PageInfo(pindex, psize, itemCount, (itemCount % psize == 0) ? (itemCount / psize) : (itemCount / psize + 1));

            return View();
        }

        /// <summary>
        ///  科研成果详情页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(int id)
        {
            FruitsModel model;
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
                else
                {
                    model = new FruitsModel();
                }
            }

            ViewBag.Fruits = model;
            return View();
        }

    }
}
