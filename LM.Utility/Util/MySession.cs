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
        public void Set<T>(string key, T value)
        {
            var expire = ConfigurationManager.AppSettings["SessionExpires"];
            int expireHours;
            expireHours = int.TryParse(expire, out expireHours) ? expireHours : 30;
            HttpContext.Current.Session[key] = value;
            HttpContext.Current.Session.Timeout = expireHours;
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
            HttpContext.Current.Session[key] = null;
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
