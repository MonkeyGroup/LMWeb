using System.Web.Mvc;

namespace LM.WebUI.Areas.Admin.Controllers
{
    public class ArticleController : WebUI.Controllers.BaseController
    {
        //
        // GET: /Article/

        public ActionResult Index()
        {
            return View();
        }

    }
}
