using Newtonsoft.Json;

namespace LM.Utility
{
    public class JsonHelper
    {
        /// <summary>
        ///  将对象序列化成 Json 格式字符串
        /// </summary>
        /// <param name="model">解析的对象</param>
        /// <returns></returns>
        public static string Serialize(object model)
        {
            return JsonConvert.SerializeObject(model);
        }

        /// <summary>
        ///  将 Json 字符串解析成一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string jsonStr)
        {
            return string.IsNullOrEmpty(jsonStr) ? default(T) : JsonConvert.DeserializeObject<T>(jsonStr);
        }


    }

}