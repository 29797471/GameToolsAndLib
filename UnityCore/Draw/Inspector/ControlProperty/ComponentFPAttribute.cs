using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 由对应字段或者属性的对象来确定属性源
    /// </summary>
    public class ComponentFPAttribute : ControlPropertyAttribute
    {
        public ComponentFPAttribute(string path, string convertMethod = null) : base(path, convertMethod)
        {
        }
        protected override void OnInit(ControlAttribute ctl)
        {
            if (ctl is ComponentPropertyAttribute)
            {
                (ctl as ComponentPropertyAttribute).SrcObj = (GameObject)GetValue();
            }
        }
    }
}
