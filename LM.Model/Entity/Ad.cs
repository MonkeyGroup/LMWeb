using System;

namespace LM.Model.Entity
{
    /// <summary>
    ///  广告
    /// </summary>
    public class Ad
    {
        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        ///  广告链接地址
        /// </summary>
        public string LinkUrl { get; set; }

        /// <summary>
        ///  广告图片路径
        /// </summary>
        public string ImgSrc { get; set; }

        /// <summary>
        ///  显示优先级，数值越大显示越靠前。
        /// </summary>
        public int Index { get; set; }

        public DateTime SaveAt { get; set; }
    }
}
