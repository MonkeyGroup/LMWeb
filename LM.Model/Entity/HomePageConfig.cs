using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LM.Model.Entity
{
    public class HomePageConfig
    {
        public int Id { set; get; }

        public string SiteName { get; set; }

        public string LogoSrc { get; set; }

        public string FlashSrc { get; set; }

        public string AdSrc { get; set; }

        public string SloganSrc { get; set; }

        public string QrSrc { get; set; }

        public string BusinessSrc { get; set; }

        public DateTime SaveTime { get; set; }
    }
}
