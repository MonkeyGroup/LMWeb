using System.Configuration;
using System.Web;

namespace LM.Utility.Util
{
    public class MySession : ISession
    {
        /// <summary>
        ///  设置一个客户端与服务器间的会话，默认有效期限30Min
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="defaultExpireMinutes"></param>
        public void Set<T>(string key, T value, int defaultExpireMinutes = 30)
        {
            var expire = ConfigurationManager.AppSettings["SessionExpires"];
            int expireMinutes;
            expireMinutes = int.TryParse(expire, out expireMinutes) ? expireMinutes : defaultExpireMinutes;
            HttpContext.Current.Session[key] = value;
            HttpContext.Current.Session.Timeout = expireMinutes;
        }

        public T Get<T>(string key)
        {
            return HttpContext.Current.Session[key] != null ? (T)HttpContext.Current.Session[key] : default(T);
        }

        public object this[string key]
        {
            get
            {
                return Get<object>(key);
            }
            set
            {
                Set(key, value);
            }
        }

        public void Clear(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                HttpContext.Current.Session.Clear();
            }
            else
            {
                HttpContext.Current.Session[key] = null;
            }
        }
        
        public string GetSessionId()
        {
            try
            {
                var cookie = HttpContext.Current.Request.Cookies["SessionId"];
                if (cookie != null)
                {
                    return cookie.Value;
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
