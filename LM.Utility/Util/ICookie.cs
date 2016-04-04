using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LM.Utility.Util
{
    public interface ICookie
    {
        /// <summary>
        ///  获取客户端请求中的指定的 cookie 值
        /// </summary>
        /// <param name="cookiename"></param>
        /// <returns></returns>
        string Get(string cookiename);

        /// <summary>
        ///  给客户端设置一个cookie，默认有效期限为6Hours
        /// </summary>
        /// <param name="cookiename"></param>
        /// <param name="cookievalue"></param>
        void Set(string cookiename, string cookievalue);

        void Clear(string cookieName);
    }
}
