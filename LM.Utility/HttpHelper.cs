using System;
using System.IO;
using System.Net;
using System.Text;

namespace LM.Utility
{
    public class HttpHelper
    {
        public static string HttpGet(string url)
        {
            var request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null) return string.Empty;
            var response = request.GetResponse() as HttpWebResponse;
            var result = string.Empty;
            if (response == null) return result;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                result = reader.ReadToEnd();
            }
            response.Close();
            return result;
        }

        public static string HttpPost(string url, string param)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(param);
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            if (request != null)
            {
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(byteArray, 0, byteArray.Length);
                }
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                string result = null;
                if (response != null)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        result = reader.ReadToEnd();
                    }
                    response.Close();
                }
                return result;
            }
            return String.Empty;
        }

        //public static async Task HttpPostAsync(string url, string param)
        //{
        //    HttpClient client = new HttpClient();
        //    try
        //    {
        //        StringContent content = new StringContent(param, Encoding.UTF8, "application/json");
        //        HttpResponseMessage response = await client.PostAsync(url, content);
        //        string result = "false";
        //        if (response.IsSuccessStatusCode)
        //        {
        //            result = await response.Content.ReadAsStringAsync();
        //        }
        //        if (result.ToLower() != "true")
        //        {
        //            throw new Exception();
        //        }
        //    }
        //    catch
        //    {
        //        LogHelper.WriteLog("同步失败\r\nUrl:" + url + "\r\nParam:" + param);
        //    }
        //}
    }
}