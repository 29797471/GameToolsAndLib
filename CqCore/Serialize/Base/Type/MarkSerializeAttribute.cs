using System;

namespace CqCore
{
    /// <summary>
    /// 修饰一个非泛型的普通类型,定义序列化时按成员属性或者字段来序列化
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct , Inherited = false, AllowMultiple = false)]
    public class MarkSerializeAttribute : Attribute
    {
        /// <summary>
        /// 定义对这个类型的序列化方式
        /// </summary>
        public SerializeTypeStyle style;
        /// <summary>
        /// 修饰一个非泛型的普通类型,定义序列化时按成员属性或者字段来序列化
        /// </summary>
        /// <param name="style">定义对这个类型的序列化方式</param>
        public MarkSerializeAttribute(SerializeTypeStyle style= SerializeTypeStyle.Field)
        {
            this.style = style;
        }
           
    }
}

