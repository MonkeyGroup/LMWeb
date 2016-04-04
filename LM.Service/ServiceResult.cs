namespace LM.Service
{
    /// <summary>
    ///  业务处理结果信息，返回的数据集不能为空，否则上层的Controller判空过于臃肿。
    ///  用法：new ServiceResult(true, "获取成功") { Data = user };
    /// </summary>
    public class ServiceResult
    {
        #region Fields
        /// <summary>
        ///  业务处理结果状态
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        ///  错误码
        /// </summary>
        public ServiceResultCode Code { get; set; }

        /// <summary>
        /// 业务处理结果信息
        /// </summary> 
        public string Message { get; set; }

        /// <summary>
        /// 数据，可以是model或者model集合。取出来需要转型
        /// </summary>
        public object Data { get; set; }
        #endregion


        #region Constructor

        public ServiceResult()
        {
            Data = default(object);
        }

        public ServiceResult(bool status, ServiceResultCode code = ServiceResultCode.正常, string message = "", object data = default(object))
        {
            Status = status;
            Code = code;
            Message = message;
            Data = data;
        }

        #endregion
    }

    public enum ServiceResultCode
    {
        #region 通用码
        正常 = 1,
        服务器异常 = 500,
        资源未找到 = 404,
        #endregion

        #region 自定义错误码

        #endregion
    }
}
