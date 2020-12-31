using System;

namespace CqCore
{
    /// <summary>
    /// 定义一个枚举值的属性标签
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    public class EnumColorAttribute : Attribute
    {
        /// <summary>
        /// 颜色名称
        /// </summary>
        public string htmlColor;
        /// <summary>
        /// 定义一个枚举值的颜色属性标签#09009
        /// </summary>
        public EnumColorAttribute(string htmlColor)
        {
            this.htmlColor = htmlColor;
        }
    }
}

