using System;
using System.Web.Mvc;
using LM.Model.Entity;
using LM.Model.Model;
using LM.Service;
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
        public ActionResult Article(ArticleModel article)
        {
            return View();
        }

        [HttpPost]
        //[Authentication]
        public JsonResult Save(ArticleModel article)
        {
            // 若无，则取出后放入session
            using (var articleService = ResolveService<ArticleService>())
            {
                var svs = articleService.Insert(new Article
                {
                    Title = article.Title,
                    Type = article.Type,
                    Author = CurrentUser.UserName,
                    Origin = article.Origin,
                    Keywords = article.Keywords,
                    Brief = article.Brief,
                    Content = article.Content,
                    IsRecommend = article.IsRecommend,
                    IsFocus = article.IsFocus,
                    IsHide = article.IsHide,
                    CreateAt = DateTime.Now,
                    ImgSrc = article.ImgSrc
                });
                return Json(new { status = svs.Status, msg = svs.Status ? "新建成功！" : "新建失败！" });
            }
        }
    }
}
