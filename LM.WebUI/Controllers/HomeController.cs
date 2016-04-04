using System.Web.Mvc;
using LM.Service.UserService;

namespace LM.WebUI.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            using (UnitOfWork)
            {
                var userService = new UserService(UnitOfWork);
                var sr = userService.GetUserList();

                ViewBag.UserList = sr.Data;
                return View();
            }
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}
