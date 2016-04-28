using System;

namespace LM.Model.Model
{
    public class BriefModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        ///  文件在服务器中的路径
        /// </summary>
        public string FilePath { get; set; }

        public DateTime SaveAt { get; set; }
    }
}
