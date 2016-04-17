using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LM.Model.Model;

namespace LM.WebUI.Controllers
{
    public class CommonController : BaseController
    {
        /// <summary>
        ///  获取联盟页面的头部信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Header()
        {
            var conf = DiMySession.Get<HomePageConfigModel>("HomePageConfig");
            ViewBag.HomePageConfig = conf ?? new HomePageConfigModel();
            return View("_Header");
        }

        /// <summary>
        ///  获取联盟底部信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Footer()
        {
            return View("_Footer");
        }

    }
}
