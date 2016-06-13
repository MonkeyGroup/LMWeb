﻿namespace LM.Model.Common
{
    /// <summary>
    ///  系统所有的枚举参数
    /// </summary>
    public enum Function
    {
        全部权限 = 1
    }

    /// <summary>
    ///  成员企业等级
    /// </summary>
    public enum MemberRange
    {
        理事长单位 = 1,
        副理事长单位 = 2,
        常务理事单位 = 3,
        理事单位 = 4
    }

    /// <summary>
    ///  成员企业类型
    /// </summary>
    public enum MemberType
    {
        材料领域企业 = 1,
        应用领域企业 = 2,
        科研协会理事单位 = 3
    }

    public enum ExpertRange
    {
        专家委员会组长 = 1,
        专家委员会副组长 = 2,
        专家委员会成员 = 3
    }

    public enum ProductType
    {
        特种气体 = 1,
        混电子化学品 = 2,
        封装材料 = 3,
        树脂 = 4,
        其他 = 5
    }

    /// <summary>
    ///  分类对象，如：产品、成员企业等
    /// </summary>
    public enum CategoryTarget
    {
        产品分类 = 1,
        成员企业分类 = 2,
    }

}

