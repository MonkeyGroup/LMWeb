using System;

namespace LM.Model.Entity
{
    public class Article
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Origin { get; set; }

        public int Hits { get; set; }

        /// <summary>
        ///  关键字，以逗号分隔（不区分中英文）
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        ///  摘要，主要内容
        /// </summary>
        public string Brief { get; set; }

        /// <summary>
        ///  正文段落
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        ///  是否推荐。
        /// 设置推荐位的文章，会取其首张图片作为滚动素材，对所有类型的文章有效
        /// </summary>
        public bool IsRecommend { get; set; }

        /// <summary>
        ///  是否关注。
        /// 特别关注位，对所有类型的文章有效
        /// </summary>
        public bool IsFocus { get; set; }

        /// <summary>
        ///  是否在幻灯片处显示。
        /// </summary>
        public bool IsShow { get; set; }

        public DateTime SaveAt { get; set; }

        /// <summary>
        ///  推荐文章在幻灯片中展示的那张图片的地址
        /// </summary>
        public string ImgSrc { get; set; }
    }
}
