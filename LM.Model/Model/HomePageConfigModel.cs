using System;

namespace LM.Model.Model
{
    public class HomePageConfigModel
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
