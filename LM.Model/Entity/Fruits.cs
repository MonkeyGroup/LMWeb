using System;

namespace LM.Model.Entity
{
    public class Fruits
    {
        public int Id { get; set; }

        /// <summary>
        ///  联盟成果名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  成果类型，手动输入
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///  研发企业，手动输入
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        ///  研发负责人
        /// </summary>
        public string Leader { get; set; }

        /// <summary>
        ///  专利情况
        /// </summary>
        public string Patent { get; set; }

        /// <summary>
        ///  知识产权
        /// </summary>
        public string Rights { get; set; }

        /// <summary>
        ///  获奖信息
        /// </summary>
        public string Awards { get; set; }

        /// <summary>
        ///  应用情况
        /// </summary>
        public string Application { get; set; }

        public string Description { get; set; }

        public DateTime SaveAt { get; set; }
    }
}