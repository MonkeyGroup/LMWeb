namespace LM.Utility.Util
{
    public interface ISession
    {
        /// <summary>
        /// 设置项
        /// 清空项时，将value设为null即可
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set<T>(string key, T value);

        /// <summary>
        /// 获取项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// Session索引器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object this[string key] { get; set; }

        /// <summary>
        ///  清除Session
        /// </summary>
        /// <param name="sessionId"></param>
        void Clear(string sessionId = null);

        /// <summary>
        ///  取Session的ID
        /// </summary>
        /// <returns></returns>
        string GetSessionId();
    }
}
