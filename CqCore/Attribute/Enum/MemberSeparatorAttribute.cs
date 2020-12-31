using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CqCore
{
    /// <summary>
    /// 定义一个数据结构在表格中被序列化时的成员分隔符
    /// </summary>
    public class MemberSeparatorAttribute: Attribute
    {
        /// <summary>
        /// 成员分隔符名称
        /// </summary>
        public string separator;
        /// <summary>
        /// 定义一个枚举值的属性标签
        /// </summary>
        public MemberSeparatorAttribute(string separator)
        {
            this.separator = separator;
        }

    }
}
