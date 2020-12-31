using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityCore
{
    /// <summary>
    /// 修饰在Inspector中的组件,定义内部属性呈现时的名称宽度
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class InpectorDrawStyleAttribute:Attribute
    {
        /// <summary>
        /// 前缀标签最小宽度
        /// </summary>
        public float minPrefixLabelWidth;
        /// <param name="minPrefixLabelWidth">前缀标签最小宽度</param>
        public InpectorDrawStyleAttribute(float minPrefixLabelWidth = 100f)
        {
            this.minPrefixLabelWidth = minPrefixLabelWidth;
        }
    }
}
