using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CqCore
{
    /// <summary>
    /// 成员属性接口
    /// </summary>
    public interface IMemberAttribute
    {
        /// <summary>
        /// 由此传入成员的父对象和成员相关信息
        /// </summary>
        void SetTarget(MemberInfo info, object parent);
    }
}
