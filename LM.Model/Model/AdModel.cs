using System;

namespace LM.Model.Model
{
    /// <summary>
    ///  广告
    /// </summary>
    public class AdModel
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

        public DateTime SaveAt { get; set; }
    }
}
