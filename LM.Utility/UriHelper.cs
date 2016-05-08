using System;
using System.Drawing;
using System.IO;
using System.Net;

namespace LM.Utility
{
    public class UriHelper
    {
        /// <summary>
        ///  从 Uri 读取图片
        /// </summary>
        /// <param name="uri">图片所在的 URI 路径</param>
        /// <param name="saveName">图片保存的名称</param>
        /// <param name="saveFolder">图片保存在本地的文件夹</param>
        /// <returns>图片保存位置</returns>
        public static string GetImages(string uri, string saveName, string saveFolder)
        {
            var imgRequst = WebRequest.Create(uri);
            var response = imgRequst.GetResponse();
            var responseStream = response.GetResponseStream();
            var fileName = string.Format("{0}{1}", DateTime.Now.ToString("HHmmssffff"), saveName);
            if (responseStream == null) return "";

            string path = saveFolder + Path.DirectorySeparatorChar + fileName;
            using (var downImage = Image.FromStream(responseStream))
            {
                if (!Directory.Exists(saveFolder))
                {
                    Directory.CreateDirectory(saveFolder);
                }
                downImage.Save(path);
            }

            return path;
        }

        /// <summary>
        ///  从 Uri 读取文件
        /// </summary>
        /// <param name="uri">文件所在的 URI 路径</param>
        /// <param name="saveName">文件保存的名称</param>
        /// <param name="saveFolder">文件保存在本地的文件夹</param>
        /// <returns>文件保存位置</returns>
        public static string GetFile(string uri, string saveName, string saveFolder)
        {
            if (!Directory.Exists(saveFolder))
            {
                Directory.CreateDirectory(saveFolder);
            }
            var r = new Random();

            var fileName = string.Format("{0}{1}{2}", DateTime.Now.ToString("HHmmssffff"), r.Next(10000), saveName);
            string path = saveFolder + Path.DirectorySeparatorChar + fileName;

            WebClient client = new WebClient();
            client.DownloadFile(uri, path);

            return path;
        }



        /// <summary>
        ///  读取网站文件
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public static bool ReadUri(string uri, string savePath)
        {
            try
            {
                // 读取 Oss 文件
                var request = (HttpWebRequest)WebRequest.Create(uri);
                var response = (HttpWebResponse)request.GetResponse();
                var stream = response.GetResponseStream();

                // 写文件
                var fileStream = File.Create(savePath);
                var buffer = new byte[1024];
                var numReadByte = 0;
                while (stream != null && (numReadByte = stream.Read(buffer, 0, 1024)) != 0)
                {
                    fileStream.Write(buffer, 0, numReadByte);
                }
                fileStream.Close();
                if (stream != null) stream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        //private void downLoad(string filename)
        //{
        //    string path = Server.MapPath("download/") + filename;
        //    FileInfo fi = new FileInfo(path);
        //    if (fi.Exists)
        //    {
        //        Response.Clear();
        //        Response.AddHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(filename));
        //        Response.AddHeader("Content-Length", fi.Length.ToString());
        //        Response.ContentType = "application/octet-stream;charset=gb2321";
        //        Response.WriteFile(fi.FullName);
        //        Response.Flush();
        //        Response.Close();
        //    }
        //}

    }
}
