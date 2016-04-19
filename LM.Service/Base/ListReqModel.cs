namespace LM.Service.Base
{
    public class ListReqModel
    {
        public int pindex { get; set; }

        public int psize { get; set; }

        /// <summary>
        ///  文章类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        ///  查询关键词串，以逗号分隔
        /// </summary>
        public string keys { get; set; }

        /// <summary>
        ///  排序字段
        /// </summary>
        public string field { get; set; }

        /// <summary>
        ///  正倒序
        /// </summary>
        public string ordertype { get; set; }

        /// <summary>
        ///  其他请求条件，以匿名类的方式使用，
        /// 如：new { IsExport = true }
        /// </summary>
        public object OtherConditions { get; set; }
    }
}
