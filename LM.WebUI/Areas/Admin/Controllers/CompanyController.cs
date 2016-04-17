using System.Web.Mvc;

namespace LM.WebUI.Areas.Admin.Controllers
{
    public class CompanyController : WebUI.Controllers.BaseController
    {
        //
        // GET: /Company/

        public ActionResult Index()
        {
            return View();
        }

    }
}
