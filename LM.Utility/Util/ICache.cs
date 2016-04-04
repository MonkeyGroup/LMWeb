using System;

namespace LM.Utility.Util
{
    public interface ICache
    {
        #region Set
        /// <summary>
        ///  将缓存对象添加至服务器内存中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="cacheVal"></param>
        void Set<T>(string cacheKey, T cacheVal);

        /// <summary>
        ///  将缓存对象添加至服务器内存中
        /// </summary>
        /// <typeparam name="T">缓存对象类型</typeparam>
        /// <param name="cacheKey">缓存名字</param>
        /// <param name="cacheVal">缓存对象</param>
        /// <param name="slidingExpiration">缓存有效期限</param>
        void Set<T>(string cacheKey, T cacheVal, TimeSpan slidingExpiration);

        /// <summary>
        ///  将缓存对象添加至服务器内存中
        /// </summary>
        /// <typeparam name="T">缓存对象类型</typeparam>
        /// <param name="cacheKey">缓存名字</param>
        /// <param name="cacheVal">缓存对象</param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="slidingExpiration">缓存有效期限</param>
        void Set<T>(string cacheKey, T cacheVal, DateTime absoluteExpiration, TimeSpan slidingExpiration);
        #endregion

        #region Remove

        /// <summary>
        /// 从Web服务器内存中移除缓存
        /// </summary>
        /// <param name="cacheKey">缓存对象的键</param>
        void Remove(string cacheKey);

        /// <summary>
        ///  清除Web服务器内存中的所有缓存
        /// </summary>
        void RemoveAll();
        #endregion

        #region Get

        /// <summary>
        ///  从Web服务器内存中取出某个缓存
        /// </summary>
        /// <typeparam name="T">此缓存对象</typeparam>
        /// <param name="cacheKey">此缓存对象所对应的键位</param>
        /// <returns></returns>
        T Get<T>(string cacheKey);

        #endregion
    }
}
