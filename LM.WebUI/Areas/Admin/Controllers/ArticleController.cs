using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LM.Model.Entity;
using LM.Model.Model;
using LM.Service;
using LM.Service.Base;
using LM.Service.Security;
using LM.WebUI.Areas.Admin.Models;

namespace LM.WebUI.Areas.Admin.Controllers
{
    public class ArticleController : BaseController
    {
        [HttpGet]
        //[Authentication]
        public ActionResult Index()
        {
            return View("List");
        }

        //[Authentication]
        public ActionResult List(string type, string keys, int pindex = 1, int psize = 10)
        {
            List<ArticleModel> articles;
            int itemCount;

            using (var articleService = ResolveService<ArticleService>())
            {
                var rs = articleService.QueryManage.GetListByPage<ArticleModel>("[Article]", "CreateAt desc", out itemCount, pindex, psize);
                articles = rs as List<ArticleModel> ?? rs.ToList();
                if (articles.Any())
                {
                    // 查询条件通过 linq 方法过滤
                    if (!string.IsNullOrEmpty(type) && articles.Count > 0)
                    {
                        articles = articles.Where(a => a.Type.ToString().Equals(type)).ToList();
                    }
                    if (!string.IsNullOrEmpty(keys) && articles.Count > 0)
                    {
                        articles = articles.Where(a => !string.IsNullOrEmpty(a.Keywords) && a.Keywords.Split(',', '，').Intersect(keys.Split(',', '，', ' ')).Any()).ToList();
                    }
                }
            }

            ViewBag.Keys = keys;
            ViewBag.Type = type;
            ViewBag.Articles = articles;
            ViewBag.PageInfo = new PageInfo(pindex, psize, itemCount, (itemCount % psize == 0) ? (itemCount / psize) : (itemCount / psize + 1));
            return View();
        }

        [HttpGet]
        //[Authentication]
        public ActionResult Article(int articleId = 0)
        {
            var article = new ArticleModel();

            // Id > 0 是编辑；Id = 0 是新建
            if (articleId > 0)
            {
                using (var articleService = ResolveService<ArticleService>())
                {
                    var rs = articleService.GetById(articleId);
                    if (rs.Status && rs.Data != null)
                    {
                        var entity = rs.Data as Article;
                        article = new ArticleModel
                        {
                            Id = entity.Id,
                            Title = entity.Title,
                            Type = entity.Type,
                            Author = CurrentUser.UserName,
                            Origin = entity.Origin,
                            Hits = entity.Hits,
                            Keywords = entity.Keywords,
                            Brief = entity.Brief,
                            Content = entity.Content,
                            IsRecommend = entity.IsRecommend,
                            IsFocus = entity.IsFocus,
                            IsHide = entity.IsHide,
                            CreateAt = DateTime.Now,
                            ImgSrc = entity.ImgSrc
                        };
                    }
                }
            }

            ViewBag.Article = article;
            return View();
        }

        [HttpPost]
        //[Authentication]
        public JsonResult Save(ArticleModel article)
        {
            // 若无，则取出后放入session
            using (var articleService = ResolveService<ArticleService>())
            {
                // Id > 0 修改，Id = 0 新增
                JsonRespModel respModel;
                if (article.Id > 0)
                {
                    var svs = articleService.Update(new Article
                    {
                        Id = article.Id,
                        Title = article.Title,
                        Type = article.Type,
                        Author = CurrentUser.UserName,
                        Origin = article.Origin,
                        Hits = article.Hits,
                        Keywords = article.Keywords,
                        Brief = article.Brief,
                        Content = article.Content,
                        IsRecommend = article.IsRecommend,
                        IsFocus = article.IsFocus,
                        IsHide = article.IsHide,
                        CreateAt = DateTime.Now,
                        ImgSrc = article.ImgSrc
                    });
                    respModel = new JsonRespModel { status = svs.Status, message = svs.Status ? "修改成功！" : "修改失败！" };
                }
                else
                {
                    var svs = articleService.Insert(new Article
                    {
                        Title = article.Title,
                        Type = article.Type,
                        Author = CurrentUser.UserName,
                        Origin = article.Origin,
                        Hits = article.Hits,
                        Keywords = article.Keywords,
                        Brief = article.Brief,
                        Content = article.Content,
                        IsRecommend = article.IsRecommend,
                        IsFocus = article.IsFocus,
                        IsHide = article.IsHide,
                        CreateAt = DateTime.Now,
                        ImgSrc = article.ImgSrc
                    });
                    respModel = new JsonRespModel { status = svs.Status, message = svs.Status ? "新建成功！" : "新建失败！" };
                }

                return Json(respModel);
            }
        }

        public ActionResult Delete(string ids, string type, string keys)
        {
            using (var articleService = ResolveService<ArticleService>())
            {
                articleService.Delete(ids.Split(',').Select(int.Parse).ToArray());
                return RedirectToAction("List", new { type = type, keys = keys });
            }
        }

    }
}
