using System.Collections.Generic;
using System.Web.Mvc;
using LM.Service.Security;
using LM.Utility;

namespace LM.WebUI.Areas.Admin.Controllers
{
    public class CommonController : BaseController
    {

        #region 文件上传

        [HttpPost]
        [Authentication]
        public JsonResult FileUpload()
        {
            var file = HttpContext.Request.Files[0]; // 默认传单个文件
            if (file == null || file.InputStream == null || string.IsNullOrEmpty(file.FileName))
            {
                return Json(new FileUploadModel { Status = false, Message = "请选择文件！" });
            }

            //定义允许上传的文件扩展名
            var extTable = new Dictionary<string, string>
            {
                {"image", "gif,jpg,jpeg,png,bmp"},
                {"flash", "swf,flv"},
                {"media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb"},
                {"file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2"}
            };

            var ext = file.FileName.ToLower().Substring(file.FileName.LastIndexOf(".") + 1);
            FileUploadModel model;
            if (extTable["image"].Contains(ext))
            {
                model = UploadHelper.FileUploadHandler(file, UploadFileTye.Img);
            }
            else if (extTable["flash"].Contains(ext))
            {
                model = UploadHelper.FileUploadHandler(file, UploadFileTye.Swf);
            }
            else if (extTable["media"].Contains(ext))
            {
                model = UploadHelper.FileUploadHandler(file, UploadFileTye.Media);
            }
            else
            {
                model = UploadHelper.FileUploadHandler(file, UploadFileTye.Other);
            }
            return Json(model);
        }


        [HttpPost]
        [Authentication]
        public void EditorFileUpload()
        {
            var file = HttpContext.Request.Files[0]; // 默认传单个文件
            if (file == null || file.InputStream == null || string.IsNullOrEmpty(file.FileName))
            {
                HttpContext.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
                HttpContext.Response.Write(JsonHelper.Serialize(new { error = 0, message = "请选择文件！！！" }));
                HttpContext.Response.End();
                return;
            }

            //定义允许上传的文件扩展名
            var extTable = new Dictionary<string, string>
            {
                {"image", "gif,jpg,jpeg,png,bmp"},
                {"flash", "swf,flv"},
                {"media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb"},
                {"file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2"}
            };

            var ext = file.FileName.ToLower().Substring(file.FileName.LastIndexOf(".") + 1);
            FileUploadModel model;
            if (extTable["image"].Contains(ext))
            {
                model = UploadHelper.FileUploadHandler(file, UploadFileTye.Img);
            }
            else if (extTable["flash"].Contains(ext))
            {
                model = UploadHelper.FileUploadHandler(file, UploadFileTye.Swf);
            }
            else if (extTable["media"].Contains(ext))
            {
                model = UploadHelper.FileUploadHandler(file, UploadFileTye.Media);
            }
            else
            {
                model = UploadHelper.FileUploadHandler(file, UploadFileTye.Other);
            }

            // 数据返回类型必须要和 Js 的控件格式一致
            var jsonStr = JsonHelper.Serialize(new { error = 0, url = model.FilePath });
            HttpContext.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            HttpContext.Response.Write(jsonStr);
            HttpContext.Response.End();
        }


        [HttpPost]
        [Authentication]
        public JsonResult BriefUpload()
        {
            var file = HttpContext.Request.Files[0]; // 默认传单个文件
            if (file == null || file.InputStream == null || string.IsNullOrEmpty(file.FileName))
            {
                return Json(new FileUploadModel { Status = false, Message = "请选择文件！" });
            }
            var model = UploadHelper.FileUploadHandler(file, UploadFileTye.Other, false);
            return Json(model);
        }

        #endregion

    }
}
