using System;

namespace UnityCore
{
    /// <summary>
    /// 编辑一个基础属性
    /// </summary>
    public class TorsionValueAttribute : ControlAttribute, IGetComMemberAttribute
    {
        public TorsionValueAttribute(string name) : base(name)
        {
        }

        public Func<object> GetOtherMemberValue { set; get; }
    }
}

