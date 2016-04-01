using System;

namespace LM.Utility.Util
{
    public interface ICache
    {
        #region Add

        /// <summary>
        /// 将对象添加到WebApp缓存中
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值，对象实例</param>
        /// <param name="expiredMinutes">默认30分钟过期</param>
        void Add<T>(string key, T value, int expiredMinutes = 30);

        #endregion

        #region Delete
        /// <summary>
        /// 从Web应用程序中移除缓存
        /// </summary>
        /// <param name="key">缓存对象的键</param>
        void Delete(string key);
        #endregion

        #region Get
        /// <summary>
        ///  从Web应用程序中取出某个缓存
        /// </summary>
        /// <typeparam name="T">此缓存对象</typeparam>
        /// <param name="key">此缓存对象所对应的键位</param>
        /// <returns></returns>
        T Get<T>(string key);
        #endregion
    }
}
