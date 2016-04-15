using System;
using System.IO;
using log4net;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = @"Logs\log4net.config", Watch = true)]
namespace LM.Utility
{
    public class LogHelper
    {
        private static ILog log = LogManager.GetLogger("Logger");

        #region 通过 log4net 记录日志
        
        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="content">内容</param>

        public static void Debug(string content)
        {
            if (string.IsNullOrEmpty(content)) return;

            log.Debug(content);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="exception">错误</param>

        public static void Debug(string content, Exception exception)
        {
            if (string.IsNullOrEmpty(content)) return;

            log.Debug(content, exception);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="content">内容</param>
        public static void Info(string content)
        {
            if (string.IsNullOrEmpty(content)) return;

            log.Info(content);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="exception">错误</param>
        public static void Info(string content, Exception exception)
        {
            if (string.IsNullOrEmpty(content)) return;

            log.Info(content, exception);
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="content">内容</param>
        public static void Error(string content)
        {
            if (string.IsNullOrEmpty(content)) return;

            log.Error(content);
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="exception">错误</param>
        public static void Error(string content, Exception exception)
        {
            if (string.IsNullOrEmpty(content)) return;

            log.Error(content, exception);
        }
        #endregion

        #region 通过本地 IO 记录日志

        public static void WriteLogByIo(Exception ex, string logPath = "")
        {
            // 路径
            var envPath = Environment.CurrentDirectory;
            envPath = envPath.Replace("/", @"\");
            var currPath = envPath.IndexOf(@"bin\Debug", StringComparison.Ordinal) > -1 ? envPath.Substring(0, envPath.IndexOf(@"bin\Debug", StringComparison.Ordinal)) : envPath;
            logPath = (logPath == "" ? currPath : logPath) + @"\Log";
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
            // 输出
            StreamWriter wt = new StreamWriter(logPath + @"\log-" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt", true);
            wt.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            wt.WriteLine("----------- " + ex.Source);
            wt.WriteLine("问题：" + ex.Message);
            wt.WriteLine(ex.StackTrace);
            wt.WriteLine("=======End\n\n");
            wt.Close();
        }

        public static void WriteLogByIo(string msg, string logPath = "")
        {
            // 路径
            var envPath = Environment.CurrentDirectory;
            envPath = envPath.Replace("/", @"\");
            var currPath = envPath.IndexOf(@"bin\Debug", StringComparison.Ordinal) > -1 ? envPath.Substring(0, envPath.IndexOf(@"bin\Debug", StringComparison.Ordinal)) : envPath;
            logPath = (logPath == "" ? currPath : logPath) + @"\Log";
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
            // 输出
            StreamWriter wt = new StreamWriter(logPath + @"\log-" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt", true);
            wt.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            wt.WriteLine(msg);
            wt.WriteLine("=======End\n\n");
            wt.Close();
        }

        #endregion

    }
}
