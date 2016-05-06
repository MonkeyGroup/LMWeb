using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System;
using System.IO;
using LM.Service;
using LM.Service.Base;
using LM.Utility;
using LM.Model.Model;
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
        ///  数据库还原
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
                    return Json(new JsonRespModel { status = false, message = "备份文件不存在，请联系系统管理员！" });
                }

                using (var dbService = ResolveService<DbProcessService>())
                {
                    var rs = dbService.RestoreDb(dbName, filePath);
                    return Json(new JsonRespModel { status = rs, message = rs ? "数据库全部以还原！" : "数据还原出现异常！" });
                }
            }
            catch (Exception e)
            {
                return Json(new JsonRespModel { status = false, message = "服务器异常！" });
            }
        }


        /// <summary>
        ///  数据库备份
        /// </summary>
        /// <returns></returns>
        [Authentication]
        public JsonResult BackupDb()
        {
            var dbName = AppSettingHelper.GetString("DbName");
            var filePath = AppSettingHelper.GetString("DbBackupFilePath");
            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    return Json(new JsonRespModel { status = false, message = "备份文件不存在，请联系系统管理员！" });
                }

                using (var dbService = ResolveService<DbProcessService>())
                {
                    var rs = dbService.BackupDb(dbName, filePath);
                    return Json(new JsonRespModel { status = rs, message = rs ? "数据库全部以还原！" : "数据还原出现异常！" });
                }
            }
            catch (Exception e)
            {
                return Json(new JsonRespModel { status = false, message = "服务器异常！" });
            }
        }

    }
}
