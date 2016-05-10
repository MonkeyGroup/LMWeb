using System.Web.Mvc;
using System;
using System.IO;
using LM.Service;
using LM.Service.Base;
using LM.Utility;
using LM.Service.Security;

namespace LM.WebUI.Areas.Admin.Controllers
{
    public class DbProcessController : BaseController
    {

        [Authentication]
        public ActionResult Index()
        {
            var filePath = AppSettingHelper.GetString("DbBackupFilePath");
            var savePath = HttpContext.Server.MapPath(filePath);
            if (System.IO.File.Exists(savePath))
            {
                ViewBag.BakPath = filePath;
            }
            ViewBag.Nav = "DbProcess";
            return View();
        }

        /// <summary>
        ///  数据库还原，还原必须要存在备份源。
        /// </summary>
        /// <returns></returns>
        [Authentication]
        public JsonResult RestoreDb()
        {
            var dbName = AppSettingHelper.GetString("DbName");
            var filePath = AppSettingHelper.GetString("DbBackupFilePath");
            var savePath = HttpContext.Server.MapPath(filePath);
            try
            {
                if (!System.IO.File.Exists(savePath))
                {
                    return Json(new JsonRespModel { status = false, message = "数据备份文件不存在，请先备份数据库！" });
                }

                using (var dbService = ResolveService<DbProcessService>())
                {
                    dbService.RestoreDb(dbName, savePath);
                    return Json(new JsonRespModel { status = true, message = "数据库已还原！" });
                }
            }
            catch (Exception e)
            {
                return Json(new JsonRespModel { status = false, message = "服务器异常：" + e.Message });
            }
        }


        /// <summary>
        ///  数据库备份，备份会新建或者覆盖原文件。
        /// </summary>
        /// <returns></returns>
        [Authentication]
        public JsonResult BackupDb()
        {
            var dbName = AppSettingHelper.GetString("DbName");
            var filePath = AppSettingHelper.GetString("DbBackupFilePath");
            var folder = HttpContext.Server.MapPath(filePath.Substring(0,filePath.LastIndexOf("/", StringComparison.Ordinal)));
            var savePath = HttpContext.Server.MapPath(filePath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            try
            {
                using (var dbService = ResolveService<DbProcessService>())
                {
                    dbService.BackupDb(dbName, savePath);
                    return Json(new JsonRespModel { status = true, message = "数据库已备份！", data = filePath });
                }
            }
            catch (Exception e)
            {
                return Json(new JsonRespModel { status = false, message = "服务器异常：" + e.Message, data = "" });
            }
        }

    }
}
