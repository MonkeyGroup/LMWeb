using System;
using System.Configuration;

namespace LM.Utility.Helper
{
    /// <summary>
    /// web.config操作类
    /// </summary>
    public sealed class ConfigHelper
    {
        /// <summary>
        /// 得到AppSettings中的配置字符串信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetConfigString(string key, string defaultValue = "")
        {
            var o = ConfigurationManager.AppSettings[key] ?? defaultValue;
            return o;
        }

        /// <summary>
        /// 得到AppSettings中的配置bool信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetConfigBool(string key)
        {
            var result = false;
            var cfgVal = GetConfigString(key);
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
        public static decimal GetConfigDecimal(string key)
        {
            decimal result = 0;
            var cfgVal = GetConfigString(key);
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
        public static int GetConfigInt(string key)
        {
            return GetConfigInt(key, 0);
        }

        /// <summary>
        /// 得到AppSettings中的配置int信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetConfigInt(string key, int defaultValue)
        {
            var result = 0;
            var cfgVal = GetConfigString(key);
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