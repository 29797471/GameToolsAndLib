using System;
using UnityEngine;
namespace UnityCore
{
    /// <summary>
    /// 选择组件属性的控件
    /// </summary>
    public class ComponentPropertyAttribute : ControlAttribute, IGetTypeAttribute
    {

        public GameObject SrcObj;
        public ComponentPropertyAttribute(string label = "") : base(label)
        {
        }

        public Func<Type> GetPropertyType { get; set; }
    }
}

