using System;
using System.Configuration;

namespace LM.Utility
{
    /// <summary>
    /// web.config操作类
    /// </summary>
    public sealed class AppSettingHelper
    {
        /// <summary>
        /// 得到AppSettings中的配置字符串信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetString(string key, string defaultValue = "")
        {
            return ConfigurationManager.AppSettings[key] ?? defaultValue;
        }

        /// <summary>
        /// 得到AppSettings中的配置bool信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetBool(string key)
        {
            var result = false;
            var cfgVal = GetString(key);
            if (string.IsNullOrEmpty(cfgVal)) return false;
            try
            {
                result = bool.Parse(cfgVal);
            }
            catch (FormatException)
            {
                // Ignore format exceptions.
            }
            return result;
        }

        /// <summary>
        /// 得到AppSettings中的配置decimal信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static decimal GetDecimal(string key)
        {
            decimal result = 0;
            var cfgVal = GetString(key);
            if (string.IsNullOrEmpty(cfgVal)) return result;
            try
            {
                result = decimal.Parse(cfgVal);
            }
            catch (FormatException)
            {
                // Ignore format exceptions.
            }

            return result;
        }

        /// <summary>
        /// 得到AppSettings中的配置int信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetInt(string key)
        {
            return GetInt(key, 0);
        }

        /// <summary>
        /// 得到AppSettings中的配置int信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetInt(string key, int defaultValue)
        {
            var result = 0;
            var cfgVal = GetString(key);
            if (string.IsNullOrEmpty(cfgVal)) return result;
            try
            {
                result = int.Parse(cfgVal);
            }
            catch (FormatException)
            {
                result = defaultValue;
            }

            return result;
        }
    }
}