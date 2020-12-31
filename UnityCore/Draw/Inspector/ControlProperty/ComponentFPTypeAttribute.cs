using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 由对应字段或者属性的类型来确定查找的类型
    /// </summary>
    public class ComponentFPTypeAttribute: ControlPropertyAttribute
    {
        
        public ComponentFPTypeAttribute(System.Type value) : base(value)
        {

        }
        public ComponentFPTypeAttribute(string path, string convertMethod = null) : base(path, convertMethod)
        {
        }

        protected override void OnInit(ControlAttribute ctl)
        {
            if (ctl is IGetTypeAttribute)
            {
                //Debug.LogError("set:" + ctl);
                (ctl as IGetTypeAttribute).GetPropertyType = () => (System.Type)GetValue();
            }
        }
    }
}
