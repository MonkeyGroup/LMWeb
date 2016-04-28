using System;

namespace LM.Model.Entity
{
    public class Brief
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
