using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LM.Utility.Helper
{
    public class HttpHelper
    {
        public static string HttpGet(string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            if (request != null)
            {
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                string result;
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
                response.Close();
                return result;
            }
            return string.Empty;
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
            return  String.Empty;
        }

        public static async Task HttpPostAsync(string url, string param)
        {
            HttpClient client = new HttpClient();
            try
            {
                StringContent content = new StringContent(param, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                string result = "false";
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
                if (result.ToLower() != "true")
                {
                    throw new Exception();
                }
            }
            catch
            {
                LogHelper.WriteLog("同步失败\r\nUrl:" + url + "\r\nParam:" + param);
            }
        }
    }
}