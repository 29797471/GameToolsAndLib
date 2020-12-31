using System;

namespace CqCore
{
    /// <summary>
    /// 定义一个类型的序列化方式和名字
    /// </summary>
    public class SerializeType
    {
        /// <summary>
        /// 可序列化的类型
        /// </summary>
        public Type type;

        /// <summary>
        /// 序列化识别名称
        /// </summary>
        public string name;

        /// <summary>
        /// 对这个类型序列化方式
        /// </summary>
        public SerializeTypeStyle style;
    }
}
