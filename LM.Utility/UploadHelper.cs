using System;
using System.IO;
using System.Web;

namespace LM.Utility
{
    public static class UploadHelper
    {
        /// <summary>
        ///  文件上传工具类。默认文件类型是非img、swf、media类型的文件。
        ///  按文件类型放入不同的文件夹
        /// </summary>
        /// <param name="file">上传的文件所在的 HttpContext.Current.Request.Files</param>
        /// <param name="type">自定义的文件类型</param>
        /// <param name="serializeName">默认使用随机数命名文件名</param>
        /// <returns></returns>
        public static FileUploadModel FileUploadHandler(HttpPostedFileBase file, UploadFileTye type = UploadFileTye.Other, bool serializeName = true)
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

            try
            {
                var fileExt = file.FileName.Substring(file.FileName.LastIndexOf(".", StringComparison.Ordinal));
                var newFileName = file.FileName;
                var directory = Path.Combine(HttpContext.Current.Server.MapPath(fileFolder), DateTime.Now.ToString("yyyyMMdd"));
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // 随机数命名
                if (serializeName)
                {
                    newFileName = new Random().Next(10000000, 99999999) + fileExt;
                    file.SaveAs(directory + "/" + newFileName);
                    return new FileUploadModel { Status = true, FilePath = fileFolder.Substring(1) + "/" + DateTime.Now.ToString("yyyyMMdd") + "/" + newFileName };
                }

                // 非随机数命名
                var filePath = directory + "/" + file.FileName;
                var index = 1;
                while (File.Exists(filePath))
                {
                    var absFileName = file.FileName.Substring(0, file.FileName.LastIndexOf(".", StringComparison.Ordinal)); // 去后缀
                    newFileName = absFileName + "(" + index++ + ")" + fileExt;
                    filePath = directory + "/" + newFileName;
                }
                file.SaveAs(filePath);
                return new FileUploadModel { Status = true, FilePath = fileFolder.Substring(1) + "/" + DateTime.Now.ToString("yyyyMMdd") + "/" + newFileName };

            }
            catch (Exception e)
            {
                return new FileUploadModel { Status = false, Message = "文件上传出现异常！" };
            }
        }


    }

    /// <summary>
    ///  上传的文件类型
    /// </summary>
    public enum UploadFileTye
    {
        Img = 1,
        Swf = 2,
        //Word = 3,
        //Pdf = 4,
        Media = 5,
        Other = 99
    }

    /// <summary>
    ///  上传结果返回模型
    /// </summary>
    public class FileUploadModel
    {
        /// <summary>
        ///  上传成功与否
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        ///  上传结果提示信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///  上传成功文件路径
        /// </summary>
        public string FilePath { set; get; }
    }

}
