using System;
using System.IO;
using log4net;

namespace LM.Utility.Helper
{
    public class LogHelper
    {
        #region 通过 log4net 记录日志
        public static void WriteLog(Exception ex)
        {
            var log = LogManager.GetLogger("Loggering");
            log.Error("Error", ex);
        }

        public static void WriteLog(string msg)
        {
            var log = LogManager.GetLogger("Loggering");
            log.Error(msg);
        }
        #endregion

        #region 通过本地 IO 记录日志

        public static void WriteLogByIo(Exception ex, string logPath = "")
        {
            // 路径
            var envPath = Environment.CurrentDirectory;
            envPath = envPath.Replace("/", @"\");
            var currPath = envPath.IndexOf(@"bin\Debug") > -1 ? envPath.Substring(0, envPath.IndexOf(@"bin\Debug")) : envPath;
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
            var currPath = envPath.IndexOf(@"bin\Debug") > -1 ? envPath.Substring(0, envPath.IndexOf(@"bin\Debug")) : envPath;
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
