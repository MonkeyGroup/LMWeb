using Newtonsoft.Json;

namespace LM.Utility.Helper
{
    public class JsonHelper
    {
        /// <summary>
        ///  将对象序列化成 Json 格式字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string Serialize<T>(T model)
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
            if (string.IsNullOrEmpty(jsonStr))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(jsonStr);
        }

        public static T Deserialize<T>(string json, params JsonConverter[] converts)
        {
            return JsonConvert.DeserializeObject<T>(json, converts);
        }
    }

}