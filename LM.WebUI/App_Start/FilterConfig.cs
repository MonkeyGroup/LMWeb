using System.Web.Mvc;
using LM.Service.Filter;

namespace LM.WebUI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ExceptionFilter());
        }
    }
}