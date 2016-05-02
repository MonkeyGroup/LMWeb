using System;

namespace LM.Model.Entity
{
    /// <summary>
    ///  分类实体。
    /// </summary>
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        ///  显示顺序，越大越优先
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        ///  给谁分类。如：产品的分类、企业的分类等
        /// </summary>
        public string Target { get; set; }

        public DateTime SaveAt { get; set; }
    }
}
