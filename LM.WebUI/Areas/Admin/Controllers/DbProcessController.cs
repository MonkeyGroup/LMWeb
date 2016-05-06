using System.Web.Mvc;
using System;
using LM.Service;
using LM.Service.Base;
using LM.Utility;
using LM.Service.Security;

namespace LM.WebUI.Areas.Admin.Controllers
{
    public class DbProcessController : BaseController
    {
        public ActionResult Index()
        {
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
            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    return Json(new JsonRespModel { status = false, message = "数据备份文件不存在，请先备份数据库！" });
                }

                using (var dbService = ResolveService<DbProcessService>())
                {
                    dbService.RestoreDb(dbName, filePath);
                    return Json(new JsonRespModel { status = true, message = "数据库已还原！" });
                }
            }
            catch (Exception e)
            {
                return Json(new JsonRespModel { status = false, message = "服务器异常！" });
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
            try
            {
                using (var dbService = ResolveService<DbProcessService>())
                {
                    dbService.BackupDb(dbName, filePath);
                    return Json(new JsonRespModel { status = true, message = "数据库已备份！" });
                }
            }
            catch (Exception e)
            {
                return Json(new JsonRespModel { status = false, message = "服务器异常！" });
            }
        }

    }
}
