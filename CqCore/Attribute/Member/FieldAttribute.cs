using System;
using System.Reflection;

namespace CqCore
{
    /// <summary>
    /// 修饰字段的标签基类
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class FieldAttribute : MemberAttribute
    {
        protected new FieldInfo Info
        {
            get { return (FieldInfo)base.Info; }
        }
        /// <summary>
        ///  字段值
        /// </summary>
        public object Target
        {
            set
            {
                ((FieldInfo)Info).SetValue(Parent, value);
            }
            get
            {
                return ((FieldInfo)Info).GetValue(Parent);
            }
        }
    }
}
