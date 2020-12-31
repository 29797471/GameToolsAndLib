using System;

namespace CqCore
{
    /// <summary>
    /// 定义一个枚举值的属性标签
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    public class EnumLabelAttribute : Attribute
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string name;

        public Type[] types;

        /// <summary>
        /// 定义一个枚举值的属性标签
        /// </summary>
        public EnumLabelAttribute(string name,params Type[] types)
        {
            this.name = name;
            this.types = types;
        }
    }
}

