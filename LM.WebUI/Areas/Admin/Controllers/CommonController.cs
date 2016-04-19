using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using LM.Model.Common;
using LM.Model.Model;
using LM.Service.Security;
using LM.Utility;

namespace LM.WebUI.Areas.Admin.Controllers
{
    public class CommonController : BaseController
    {

        #region 文件上传

        [HttpPost]
        //[Authentication]
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
                model = FileUploadHandler(file, UploadFileTye.Img);
            }
            else if (extTable["flash"].Contains(ext))
            {
                model = FileUploadHandler(file, UploadFileTye.Swf);
            }
            else if (extTable["media"].Contains(ext))
            {
                model = FileUploadHandler(file, UploadFileTye.Media);
            }
            else
            {
                model = FileUploadHandler(file, UploadFileTye.Other);
            }
            return Json("aaaaaaaaaaaa");
        }


        [HttpPost]
        //[Authentication]
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
                model = FileUploadHandler(file, UploadFileTye.Img);
            }
            else if (extTable["flash"].Contains(ext))
            {
                model = FileUploadHandler(file, UploadFileTye.Swf);
            }
            else if (extTable["media"].Contains(ext))
            {
                model = FileUploadHandler(file, UploadFileTye.Media);
            }
            else
            {
                model = FileUploadHandler(file, UploadFileTye.Other);
            }

            var jsonStr = JsonHelper.Serialize(new { error = 0, url = model.FilePath });
            HttpContext.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            HttpContext.Response.Write(jsonStr);
            HttpContext.Response.End();
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
                case UploadFileTye.Media:
                    fileFolder = "~/Upload/media"; break;
                case UploadFileTye.Other:
                    fileFolder = "~/Upload/other"; break;
                default:
                    fileFolder = "~/Upload/other"; break;
            }

            var fileExt = file.FileName.Substring(file.FileName.LastIndexOf(".", StringComparison.Ordinal));
            var newFileName = new Random().Next(10000000, 99999999) + fileExt;
            var directory = Path.Combine(Request.MapPath(fileFolder), DateTime.Now.ToString("yyyyMMdd"));
            try
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                file.SaveAs(directory + "/" + newFileName);
                return new FileUploadModel { Status = true, FilePath = fileFolder.Substring(1) + "/" + DateTime.Now.ToString("yyyyMMdd") + "/" + newFileName };
            }
            catch (Exception e)
            {
                return new FileUploadModel { Status = false, Message = "文件上传出现异常！" };
            }
        }

        #endregion

    }
}
