using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using LM.Model.Common;
using LM.Model.Model;
using LM.Utility;

namespace LM.WebUI.Areas.Admin.Controllers
{
    public class CommonController : WebUI.Controllers.BaseController
    {

        #region 文件上传
        [HttpPost]
        public JsonResult ImgUpload()
        {
            var file = HttpContext.Request.Files["Filedata"];
            var model = FileUploadHandler(file, UploadFileTye.Img);
            return Json(model);
        }

        [HttpPost]
        public JsonResult SwfUpload()
        {
            var file = HttpContext.Request.Files["Filedata"];
            var model = FileUploadHandler(file, UploadFileTye.Swf);
            return Json(model);
        }

        /// <summary>
        ///  按文件类型放入不同的文件夹
        /// </summary>
        /// <param name="file"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private FileUploadModel FileUploadHandler(HttpPostedFileBase file, UploadFileTye type)
        {
            if (file == null || string.IsNullOrEmpty(file.FileName.Trim()))
            {
                return new FileUploadModel { Status = false, Message = "请选择文件！" };
            }

            string fileFolder;
            switch (type)
            {
                case UploadFileTye.Img:
                    fileFolder = "~/Upload/images"; break;
                case UploadFileTye.Swf:
                    fileFolder = "~/Upload/swf"; break;
                case UploadFileTye.Word:
                    fileFolder = "~/Upload/word"; break;
                case UploadFileTye.Pdf:
                    fileFolder = "~/Upload/pdf"; break;
                default:
                    fileFolder = "~/Upload"; break;
            }

            var fileExt = file.FileName.Substring(file.FileName.LastIndexOf(".", StringComparison.Ordinal));
            var newFileName = file.FileName.Replace(fileExt, string.Empty) + new Random().Next(10000000, 99999999) + fileExt;
            var filePath = Path.Combine(Request.MapPath(fileFolder), Path.GetFileName(newFileName));
            try
            {
                file.SaveAs(filePath);
                return new FileUploadModel { Status = true, FilePath = fileFolder.Substring(1) + "/" + newFileName };
            }
            catch
            {
                return new FileUploadModel { Status = false, Message = "文件上传出现异常！" };
            }
        }

        #endregion


    }
}
