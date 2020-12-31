using System;

namespace CqCore
{
    /// <summary>
    /// 定义一个枚举值的属性标签
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    internal class CalcEnumAttribute : Attribute
    {
        /// <summary>
        /// 优先级
        /// </summary>
        public uint pri;

        /// <summary>
        /// 运算符
        /// </summary>
        public string op;

        /// <summary>
        /// 可交换
        /// </summary>
        public bool swap;

        /// <summary>
        /// 编译器将操作符转换的反射调用函数名称
        /// </summary>
        public string methodName;

        public CalcOperator value;

        public CalcEnumAttribute(string op, uint pri,string methodName,bool swap=true)
        {
            this.pri = pri;
            this.op = op;
            this.methodName = methodName;
            this.swap = swap;
        }
    }
}

