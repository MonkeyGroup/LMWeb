using System;

namespace LM.Model.Model
{
    public class BaseInfoModel
    {
        public int Id { get; set; }

        public string Address { get; set; }

        public string Telphone { get; set; }

        public string Fax { get; set; }

        public string Site { get; set; }

        public string Email { get; set; }

        /// <summary>
        ///  百度地图的Html
        /// </summary>
        public string Map { get; set; }

        /// <summary>
        ///  联盟介绍的文章内容
        /// </summary>
        public string Introduce { get; set; }

        /// <summary>
        ///  联盟章程
        /// </summary>
        public string Chapter { get; set; }

        /// <summary>
        ///  联盟机构
        /// </summary>
        public string Organize { get; set; }

        /// <summary>
        ///  入门须知
        /// </summary>
        public string Notice { get; set; }

        /// <summary>
        ///  申请流程
        /// </summary>
        public string Process { get; set; }

        public DateTime SaveAt { get; set; }

        ///// <summary>
        /////  申请材料的绝对路径
        ///// </summary>
        public string ApplyFilePath { get; set; }
    }
}
