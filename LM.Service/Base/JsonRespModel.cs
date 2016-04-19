namespace LM.Service.Base
{
    public class JsonRespModel
    {
        /// <summary>
        ///  服务器响应状态，成功与否
        /// </summary>
        public bool status { get; set; }

        /// <summary>
        ///  错误码，部分地方使用
        /// </summary>
        public int error { get; set; }

        /// <summary>
        ///  响应提示信息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        ///  服务器返回数据
        /// </summary>
        public object data { get; set; }
    }
}
