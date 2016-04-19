using System;
using System.Web.Mvc;
using LM.Model.Entity;
using LM.Model.Model;
using LM.Service;
using LM.Service.Base;
using LM.Service.Security;

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
        public ActionResult List()
        {
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
            JsonRespModel respModel = null;
            // 若无，则取出后放入session
            using (var articleService = ResolveService<ArticleService>())
            {
                // Id > 0 修改，Id = 0 新增
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
    }
}
