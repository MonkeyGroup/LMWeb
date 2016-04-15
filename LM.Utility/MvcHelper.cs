using System;
using System.Web;
using System.Web.Mvc;

namespace LM.Utility
{
    public static class MvcHelper
    {
        /// <summary>
        ///  资源文件所在域
        /// </summary>
        public static string ResourceDoMain
        {
            get
            {
                return AppSettingHelper.GetString("ResourceDoMain", string.Empty);
            }
        }

        /// <summary>
        ///  Web应用程序版本
        /// </summary>
        public static string Version
        {
            get
            {
                return AppSettingHelper.GetString("Version", "1.0");
            }
        }

        /// <summary>
        /// 生成排序HTML标题代码
        /// </summary>
        /// <param name="title"></param>
        /// <param name="curSortField">类型</param>
        /// <param name="sortField">当前排序的类型，有服务端给出</param>
        /// <param name="isAsc">排序方式</param>
        /// <param name="doFunc">请求执行的js方法</param>
        /// <returns></returns>
        public static string ToggleListSortHtml(string title, string curSortField, string sortField, bool isAsc, string doFunc)
        {
            string html;

            //第一次加载页面sortType可能为空，隐藏所有排序图标
            if (string.IsNullOrEmpty(sortField))
            {
                html = string.Format("<span class=\"table-title\">{2}</span><em class=\"icon-sort\" onclick=\"{0}('{1}',1)\"></em>", doFunc, curSortField, title);
                //html = string.Format("<div class=\"Sortnormal\">{2}</div>", doFunc, curSortType, title);
            }
            else
            {
                if (curSortField == sortField && isAsc)
                {
                    html = string.Format("<span class=\"table-title\">{2}</span><em class=\"icon-sort icon-up\" onclick=\"{0}('{1}',0)\"></em>", doFunc, curSortField, title);

                    //html = string.Format("<div class=\"Sortup\" onclick=\"{0}('{1}','desc')\">{2}<div>", doFunc, curSortType, title);
                }
                else if (curSortField == sortField && !isAsc)
                {
                    html = string.Format("<span class=\"table-title\">{2}</span><em class=\"icon-sort icon-down\" onclick=\"{0}('{1}',1)\"></em>", doFunc, curSortField, title);
                    //html = string.Format("<div class=\"Sortdown\" onclick=\"{0}('{1}','asc')\">{2}<div>", doFunc, curSortType, title);
                }
                else
                {
                    html = string.Format("<span class=\"table-title\">{2}</span><em class=\"icon-sort\" onclick=\"{0}('{1}',1)\"></em>", doFunc, curSortField, title);
                    //html = string.Format("<div class=\"Sortnormal\" onclick=\"{0}('{1}','desc')\">{2}</div>", doFunc, curSortType, title);
                }
            }
            return html;
        }

        /// <summary>
        /// 在页面输出样式、脚本等文件的引用。
        /// 如：
        /// 《script src="1.js" type="text/javascript"》，
        /// 《link href="1.css" rel="stylesheet" type="text/css" /》 
        /// </summary>
        /// <param name="html">视图的 Html 控件</param>
        /// <param name="formatStr">字符串format语句</param>
        /// <param name="link">脚本样式连接</param>
        /// <returns></returns>
        private static IHtmlString Render(this HtmlHelper html, string formatStr, string link)
        {
            //var ticks = DateTime.Now.ToString("yyyyMMddHH");
            //文件引用后缀改为版本号形式
            var ticks = "version=" + Version;
            //资源文件站点路径不带"/" 例:http://localhost:9999
            var doMain = ResourceDoMain;
            return html.Raw(string.Format(formatStr, doMain, link, ticks));
        }

        /// <summary>
        /// 在页面输出脚本文件的引用。
        /// 如：
        /// &lt;script src="/script/1.js" type="text/javascript" &gt;
        /// </summary>
        /// <param name="html">视图的 Html 控件</param>
        /// <param name="link">文件路径，相对于web文件的路径</param>
        /// <returns></returns>
        public static IHtmlString RenderScript(this HtmlHelper html, string link)
        {
            const string formatStr = "<script src=\"{0}{1}?{2}\" type=\"text/javascript\"></script>";
            return Render(html, formatStr, link);
        }

        /// <summary>
        /// 在页面输出requirejs脚本 附加data-main脚本文件的引用。
        /// 如：
        /// &lt;script src="/script/require.js" type="text/javascript" data-main="/script/app/main.js" &gt;
        /// </summary>
        /// <param name="html">视图的 Html 控件</param>
        /// <param name="link">requireJs文件路径，相对于web文件的路径</param>
        /// <param name="main">mainJs文件路径，相对于web文件的路径</param>
        public static IHtmlString RenderRequireScript(this HtmlHelper html, string link, string main)
        {
            const string formatStr = "<script src=\"{0}{1}?{2}\" type=\"text/javascript\" data-main=\"{3}.js\"></script>";
            //文件引用后缀改为版本号形式
            var ticks = "version=" + Version;
            //资源文件站点路径不带"/" 例:http://localhost:9999
            var doMain = ResourceDoMain;
            //requireJs baseURL
            var baseUrl = "/Scripts";
            if (doMain != string.Empty)
            {
                baseUrl = doMain + baseUrl;
            }

            var requireJsGlobalConfig = @"<script>var require = {urlArgs:'" + ticks + @"',baseUrl:'" + baseUrl
                                           + @"'};</script>";
            return html.Raw(requireJsGlobalConfig + String.Format(formatStr, doMain, link, ticks, doMain + main));
        }

        /// <summary>
        /// 在页面输出样式表文件的引用。
        /// 如：
        /// &lt;link href="/css/1.css" rel="stylesheet" type="text/css"&gt;
        /// </summary>
        /// <param name="html">视图的 Html 控件</param>
        /// <param name="link">文件路径，相对于web文件的路径</param>
        public static IHtmlString RenderCss(this HtmlHelper html, string link)
        {
            string formatStr = "<link href=\"{0}{1}?{2}\" rel=\"stylesheet\" type=\"text/css\" />";
            return Render(html, formatStr, link);
        }

    }
}
