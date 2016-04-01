using System;
using System.Web;

namespace LM.Utility.Util
{
    public class AppCache : ICache
    {
        public void Add<T>(string key, T value, int expiredMinutes = 30)
        {
            var cache = HttpRuntime.Cache;
            cache.Insert(key, value, null, DateTime.Now.AddMinutes(expiredMinutes), TimeSpan.Zero);
        }

        public T Get<T>(string key)
        {
            var cache = HttpRuntime.Cache;
            return (T)cache.Get(key);
        }

        public void Delete(string key)
        {
            var cache = HttpRuntime.Cache;
            cache.Remove(key);
        }

    }
}
