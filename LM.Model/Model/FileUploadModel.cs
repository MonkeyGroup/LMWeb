using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LM.Model.Model
{
    public class FileUploadModel
    {
        /// <summary>
        ///  上传成功与否
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        ///  上传结果提示信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///  上传成功文件路径
        /// </summary>
        public string FilePath { set; get; }
    }


}
