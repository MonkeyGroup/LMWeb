using System;
using System.Collections.Generic;
using System.Web.Mvc;
using LM.Model.Entity;
using LM.Model.Model;
using LM.Service;
using LM.WebUI.Areas.Admin.Models;

namespace LM.WebUI.Controllers
{
    public class ArticleController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToAction("InfoList");
        }


        #region 联盟动态 & 行业信息 | 联盟简报

        /// <summary>
        ///  文章列表。默认类型为“联盟动态”。
        ///  类型有：“联盟动态”、“行业信息”、“联盟要闻”、“特别关注”。
        /// 条件：
        /// 1. 非隐藏；
        /// 2. 类型为“联盟动态”；
        /// 排序优先级：
        /// 1. 编辑日期倒序；
        /// 2. 推荐与否；
        /// </summary>
        /// <returns></returns>
        public ActionResult InfoList(string type = "联盟动态", int pindex = 1)
        {
            List<ArticleModel> models;
            int itemCount, psize;
            var where = "where 1=1 ";
            if ("联盟要闻".Equals(type))
            {
                where += @" and a.Type = '联盟动态' and a.IsRecommend = 1";
            }
            else if ("特别关注".Equals(type))
            {
                where += @" and a.Type = '行业信息' and a.IsFocus = 1";
            }
            else
            {
                where += string.Format(" and a.Type = '{0}'", type);
            }

            using (var articleService = ResolveService<ArticleService>())
            {
                psize = 15;
                var query = string.Format(@"(select a.Id, a.Type, a.Title, a.SaveAt, a.IsRecommend from [Article] a {0})", where);
                var orderby = "SaveAt desc, IsRecommend desc";
                var rs = articleService.GetByPage(query, orderby, pindex, psize, out itemCount);
                models = rs.Status ? rs.Data as List<ArticleModel> : new List<ArticleModel>();
            }

            var briefs = new List<BriefModel>(); // 3条简报

            using (var briefService = ResolveService<BriefService>())
            {
                var rs = briefService.GetList("select top 3 Id,Name,FilePath from [Brief] order by SaveAt desc");
                if (rs.Status && rs.Data != null)
                {
                    briefs = rs.Data as List<BriefModel>;
                }
            }

            ViewBag.Briefs = briefs;
            ViewBag.Type = type;
            ViewBag.Models = models;
            ViewBag.PageInfo = new PageInfo(pindex, psize, itemCount, (itemCount % psize == 0) ? (itemCount / psize) : (itemCount / psize + 1));

            //if (type == "科研成果" || type == "寻求合作")
            //{
            //    return View("PerfomList");
            //}
            return View();
        }

        /// <summary>
        ///  联盟简报列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pindex"></param>
        /// <returns></returns>
        public ActionResult BriefList(string type = "联盟动态", int pindex = 1)
        {
            var models = new List<BriefModel>();
            int itemCount, psize;

            using (var briefService = ResolveService<BriefService>())
            {
                psize = 15;
                var query = @"(select a.Id, a.FilePath, a.Name, a.SaveAt from [Brief] a)";
                var orderby = "SaveAt desc, Id desc";
                var rs = briefService.GetByPage(query, orderby, pindex, psize, out itemCount);
                models = rs.Status ? rs.Data as List<BriefModel> : models;
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
        ///  文章详情页，联盟动态 和 科研合作通用。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(int id)
        {
            ArticleModel model;
            using (var articleService = ResolveService<ArticleService>())
            {
                var rs = articleService.GetById(id);
                if (rs.Status && rs.Data != null)
                {
                    var entity = rs.Data as Article;
                    model = new ArticleModel
                    {
                        Id = entity.Id,
                        Title = entity.Title,
                        Type = entity.Type,
                        Author = entity.Author,
                        Origin = entity.Origin,
                        Hits = entity.Hits,
                        Keywords = entity.Keywords,
                        Brief = entity.Brief,
                        Content = entity.Content,
                        IsRecommend = entity.IsRecommend,
                        IsFocus = entity.IsFocus,
                        IsShow = entity.IsShow,
                        SaveAt = entity.SaveAt,
                        ImgSrc = entity.ImgSrc
                    };
                    // 浏览量+1
                    articleService.Update(new { Id = entity.Id, Hits = entity.Hits + 1 });
                }
                else
                {
                    model = new ArticleModel();
                }
            }

            ViewBag.Article = model;
            return View();
        }

        #endregion


    }
}
