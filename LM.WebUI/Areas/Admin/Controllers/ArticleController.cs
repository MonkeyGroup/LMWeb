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
    public class ArticleController : BaseController
    {
        [HttpGet]
        [Authentication]
        public ActionResult Index()
        {
            return View("List");
        }

        [HttpGet]
        [Authentication]
        public ActionResult List(string type, string keys, int pindex = 1, int psize = 2)
        {
            List<ArticleModel> models;
            int itemCount;

            using (var articleService = ResolveService<ArticleService>())
            {
                // 按条件查询 sql
                var where = " where 1=1 ";
                if (!string.IsNullOrEmpty(type))
                {
                    where += string.Format(" and a.Type = '{0}' ", type);
                }
                if (!string.IsNullOrEmpty(keys))
                {
                    var keyArr = keys.Split(',', '，', ' ');
                    where += " and ( 1=2 ";
                    where = keyArr.Aggregate(where, (current, key) => current + string.Format(" or CHARINDEX('{0}',a.Title)>0 or CHARINDEX('{0}',a.Keywords)>0 ", key.Trim()));
                    where += ")";
                }

                psize = AppSettingHelper.GetInt("PageSize") != 0 ? AppSettingHelper.GetInt("PageSize") : pindex;
                var query = string.Format(@"(select a.* from [Article] a {0})", where);
                var rs = articleService.GetByPage(query, "SaveAt desc", pindex, psize, out itemCount);
                models = rs.Status ? rs.Data as List<ArticleModel> : new List<ArticleModel>();

                articleService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", rs.Status ? "查询成功！" : "查询失败！", "文章列表页") });
            }

            ViewBag.Keys = keys;
            ViewBag.Type = type;
            ViewBag.Articles = models;
            ViewBag.PageInfo = new PageInfo(pindex, psize, itemCount, (itemCount % psize == 0) ? (itemCount / psize) : (itemCount / psize + 1));
            ViewBag.Nav = "ArticleList";
            return View();
        }

        [HttpGet]
        [Authentication]
        public ActionResult Article(int id = 0)
        {
            var model = new ArticleModel { Hits = new Random().Next(0, 500) };

            // Id > 0 是编辑；Id = 0 是新建
            if (id > 0)
            {
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
                            Author = CurrentUser.UserName,
                            Origin = entity.Origin,
                            Hits = entity.Hits,
                            Keywords = entity.Keywords,
                            Brief = entity.Brief,
                            Content = entity.Content,
                            IsRecommend = entity.IsRecommend,
                            IsFocus = entity.IsFocus,
                            IsHide = entity.IsHide,
                            SaveAt = DateTime.Now,
                            ImgSrc = entity.ImgSrc
                        };
                    }
                }
            }

            ViewBag.Article = model;
            ViewBag.Nav = "ArticleList";
            return View();
        }

        [HttpPost]
        [Authentication]
        public JsonResult Save(ArticleModel model)
        {
            // 若无，则取出后放入session
            using (var articleService = ResolveService<ArticleService>())
            {
                // Id > 0 修改，Id = 0 新增
                JsonRespModel respModel;
                if (model.Id > 0)
                {
                    var svs = articleService.Update(new Article
                    {
                        Id = model.Id,
                        Title = model.Title,
                        Type = model.Type,
                        Author = CurrentUser.UserName,
                        Origin = model.Origin,
                        Hits = model.Hits,
                        Keywords = model.Keywords,
                        Brief = model.Brief,
                        Content = model.Content,
                        IsRecommend = model.IsRecommend,
                        IsFocus = model.IsFocus,
                        IsHide = model.IsHide,
                        SaveAt = DateTime.Now,
                        ImgSrc = model.ImgSrc
                    });
                    respModel = new JsonRespModel { status = svs.Status, message = svs.Status ? "修改成功！" : "修改失败！" };
                    articleService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", svs.Status ? "修改成功！" : "修改失败！", "文章修改页") });
                }
                else
                {
                    var svs = articleService.Insert(new Article
                    {
                        Title = model.Title,
                        Type = model.Type,
                        Author = CurrentUser.UserName,
                        Origin = model.Origin,
                        Hits = model.Hits,
                        Keywords = model.Keywords,
                        Brief = model.Brief,
                        Content = model.Content,
                        IsRecommend = model.IsRecommend,
                        IsFocus = model.IsFocus,
                        IsHide = model.IsHide,
                        SaveAt = DateTime.Now,
                        ImgSrc = model.ImgSrc
                    });
                    respModel = new JsonRespModel { status = svs.Status, message = svs.Status ? "新建成功！" : "新建失败！" };
                    articleService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", svs.Status ? "新建成功！" : "新建失败！", "文章新建页") });
                }

                return Json(respModel);
            }
        }

        [HttpPost]
        [Authentication]
        public ActionResult Delete(string ids, string type, string keys)
        {
            using (var articleService = ResolveService<ArticleService>())
            {
                var rs = articleService.Delete(ids.Split(',').Select(int.Parse).ToArray());
                articleService.WriteLog(new OperationLog { User = CurrentUser.UserName, Ip = HttpContext.Request.UserHostAddress, Operation = string.Format("{1}，{0}", rs.Status ? "删除成功！" : "删除失败！", "文章删除页") });
                return RedirectToAction("List", new { type = type, keys = keys });
            }
        }

    }
}
