using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LM.WebUI.Areas.Admin.Models
{
    public class Page
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public int ItemCount { get; set; }
    }
}