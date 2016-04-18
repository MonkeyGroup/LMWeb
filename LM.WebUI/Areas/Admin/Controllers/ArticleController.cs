using System.Web.Mvc;
using LM.Model.Model;
using LM.Service.Security;

namespace LM.WebUI.Areas.Admin.Controllers
{
    public class ArticleController : WebUI.Controllers.BaseController
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
        public ActionResult Save(ArticleModel article)
        {
            return RedirectToAction("Index");
        }
    }
}
