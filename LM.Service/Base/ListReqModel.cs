namespace LM.Service.Base
{
    public class ListReqModel
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        /// <summary>
        ///  查询关键词串
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        ///  排序字段
        /// </summary>
        public string OrderField { get; set; }

        /// <summary>
        ///  正倒序
        /// </summary>
        public string OrderType { get; set; }

        /// <summary>
        ///  其他请求条件，以匿名类的方式使用，
        /// 如：new { IsExport = true }
        /// </summary>
        public object OtherConditions { get; set; }
    }
}
