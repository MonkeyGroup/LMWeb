using System;
using System.Configuration;
using System.Web;

namespace LM.Utility.Util
{
    public class MyCookie : ICookie
    {
        /// <summary>
        /// 清除指定Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        public void Clear(string cookieName)
        {
            var cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddYears(-3);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// 获取客户端请求中的指定的 cookie 值
        /// </summary>
        /// <param name="cookiename">cookiename</param>
        /// <returns></returns>
        public string Get(string cookiename)
        {
            var cookie = HttpContext.Current.Request.Cookies[cookiename];
            string str = string.Empty;
            if (cookie != null)
            {
                str = cookie.Value;
            }
            return str;
        }

        /// <summary>
        /// 给客户端设置一个cookie，默认有效期限为24Hours
        /// </summary>
        /// <param name="cookiename"></param>
        /// <param name="cookievalue"></param>
        public void Set(string cookiename, string cookievalue)
        {
            var expire = ConfigurationManager.AppSettings["CookieExpireHours"];
            double expireHours;
            expireHours = double.TryParse(expire, out expireHours) ? expireHours : 24.0;
            SetCookie(cookiename, cookievalue, DateTime.Now.AddHours(expireHours));
        }

        /// <summary>
        /// 添加一个Cookie
        /// </summary>
        /// <param name="cookiename">cookie名</param>
        /// <param name="cookievalue">cookie值</param>
        /// <param name="expires">过期时间 DateTime</param>
        private static void SetCookie(string cookiename, string cookievalue, DateTime expires)
        {
            var cookie = new HttpCookie(cookiename)
            {
                Value = cookievalue,
                Expires = expires
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}
