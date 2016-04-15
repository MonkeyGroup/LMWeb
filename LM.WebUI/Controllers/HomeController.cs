using System.Collections.Generic;
using System.Web.Mvc;
using LM.Model.Entity;
using LM.Model.Model;
using LM.Service;
using LM.Utility;

namespace LM.WebUI.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            using (var userService = ResolveService<UserService>())
            {

                return View();
            }
        }

        public ActionResult About()
        {
            using (var userService = ResolveService<UserService>())
            {
                var sr = userService.GetByPage(1, 8);
                var userList = new List<UserModel>();
                ((List<User>)sr.Data).ForEach<User>(u => { userList.Add(new UserModel { Id = u.Id, Age = u.Age, Name = u.Name }); });
                ViewBag.UserList = userList;
                return View();
            }
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}
