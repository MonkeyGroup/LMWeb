using System;
using System.Web;

namespace LM.Utility.Util
{
    public class MyCache : ICache
    {
        public void Set<T>(string cacheKey, T cacheVal)
        {
            var cache = HttpRuntime.Cache;
            cache.Insert(cacheKey, cacheVal);
        }

        public void Set<T>(string cacheKey, T cacheVal, TimeSpan slidingExpiration)
        {
            var cache = HttpRuntime.Cache;
            cache.Insert(cacheKey, cacheVal, null, DateTime.MaxValue, slidingExpiration, System.Web.Caching.CacheItemPriority.NotRemovable, null);
        }

        public void Set<T>(string cacheKey, T cacheVal, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            var cache = HttpRuntime.Cache;
            cache.Insert(cacheKey, cacheVal, null, absoluteExpiration, slidingExpiration);
        }

        public void Remove(string cacheKey)
        {
            var cache = HttpRuntime.Cache;
            cache.Remove(cacheKey);
        }

        public void RemoveAll()
        {
            var cache = HttpRuntime.Cache;
            var cacheEnum = cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                cache.Remove(cacheEnum.Key.ToString());
            }
        }

        public T Get<T>(string cacheKey)
        {
            var cache = HttpRuntime.Cache;
            return cache[cacheKey] != null ? (T)cache[cacheKey] : default(T);
        }
    }
}
