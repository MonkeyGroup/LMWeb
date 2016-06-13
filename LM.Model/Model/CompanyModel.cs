using System;
using LM.Model.Common;

namespace LM.Model.Model
{
    public class CompanyModel
    {
        public int Id { get; set; }

        /// <summary>
        ///  成员类型：上游企业、下游企业、科研院所协会
        /// </summary>
        public MemberType Type { get; set; }

        public string Name { get; set; }

        public string Site { get; set; }

        public string LogoSrc { get; set; }

        public string Description { get; set; }

        /// <summary>
        ///  成员分类：理事长企业、理事企业等。
        ///  数据取自 Category 的 Id
        /// </summary>
        public int Range { get; set; }

        public string RangeName { get; set; }

        public DateTime SaveAt { get; set; }

        /// <summary>
        ///  排序优先级
        /// </summary>
        public int Index { get; set; }

    }


}
