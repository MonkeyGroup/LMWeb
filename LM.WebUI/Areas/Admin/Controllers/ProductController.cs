using System.Web.Mvc;

namespace LM.WebUI.Areas.Admin.Controllers
{
    public class ProductController : WebUI.Controllers.BaseController
    {
        //
        // GET: /Product/

        public ActionResult Index()
        {
            return View();
        }

    }
}
