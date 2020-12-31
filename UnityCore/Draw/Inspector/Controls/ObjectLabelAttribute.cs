
using System;

namespace UnityCore
{
    /// <summary>
    /// 编辑一个基础属性
    /// </summary>
    public class ObjectLabelAttribute : ControlAttribute, IGetTypeAttribute
    {
        public ObjectLabelAttribute(string name) : base(name)
        {
        }

        public Func<Type> GetPropertyType { get; set; }
    }
}
