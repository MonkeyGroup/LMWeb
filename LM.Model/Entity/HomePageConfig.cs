using System;

namespace LM.Model.Entity
{
    public class HomePageConfig
    {
        public int Id { set; get; }

        public string SiteName { get; set; }

        public string LogoSrc { get; set; }

        public string FlashSrc { get; set; }

        public string QrSrc { get; set; }

        public string AdSrc { get; set; }

        public string AdLink { get; set; }

        public string SloganSrc { get; set; }

        public string SloganLink { get; set; }

        public string BusinessSrc { get; set; }

        public string BusinessLink { get; set; }

        public string Product1Src { get; set; }

        public string Product1Link { get; set; }

        public string Product2Src { get; set; }

        public string Product2Link { get; set; }

        public string Product3Src { get; set; }

        public string Product3Link { get; set; }

        public string Product4Src { get; set; }

        public string Product4Link { get; set; }

        public DateTime SaveAt { get; set; }
    }
}
