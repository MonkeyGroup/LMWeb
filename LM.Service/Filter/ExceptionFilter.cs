﻿using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using LM.Utility;

namespace LM.Service.Filter
{
    public class ExceptionFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                var sb = new StringBuilder();
                sb.Append("|Url:");
                if (filterContext.HttpContext.Request.Url != null)
                    sb.Append(filterContext.HttpContext.Request.Url.AbsoluteUri);
                sb.Append("\r\n");
                sb.Append(filterContext.Exception.Message.Replace("<", string.Empty).Replace(">", string.Empty));
                sb.Append("\r\n");
                sb.Append(filterContext.Exception.Source);
                sb.Append("\r\n");
                sb.Append(filterContext.Exception.StackTrace);
                sb.Append("\r\n");
                Exception ex = filterContext.Exception.InnerException;
                while (ex != null)
                {
                    sb.Append("\r\n-----------------------");
                    sb.Append(filterContext.Exception.Message.Replace("<", string.Empty).Replace(">", string.Empty));
                    sb.Append("\r\n");
                    sb.Append(filterContext.Exception.Source);
                    sb.Append("\r\n");
                    sb.Append(filterContext.Exception.StackTrace);
                    ex = ex.InnerException;
                }

                var sbHeaders = new StringBuilder();
                for (var i = 0; i < filterContext.HttpContext.Request.Headers.Keys.Count; i++)
                {
                    var keyName = filterContext.HttpContext.Request.Headers.AllKeys[i];
                    var strings = filterContext.HttpContext.Request.Headers.GetValues(keyName);
                    if (strings != null)
                        sbHeaders.Append(keyName + ":" + strings[0] + " ");
                }

                var logPath = string.Empty;
                if (Directory.Exists("D://"))
                {
                    logPath = "D://";
                }
                else if (Directory.Exists("E://"))
                {
                    logPath = "E://";
                }
                else if (Directory.Exists("F://"))
                {
                    logPath = "F://";
                }

                LogHelper.WriteLogByIo(sb.ToString(), logPath);

                filterContext.ExceptionHandled = true;

                //普通请求，返回自定义错误页面
                filterContext.Result = new ContentResult { Content = "系统异常" };

            }
        }

    }
}