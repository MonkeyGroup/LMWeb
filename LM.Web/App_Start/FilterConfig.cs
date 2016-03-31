using System.Web.Mvc;
using LM.Web.Filters;

namespace LM.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ExceptionFilter());
        }
    }
}