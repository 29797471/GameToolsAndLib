using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 当字段变化时设置属性或者调用方法
    /// </summary>
    public class OnValueChangedAttribute : ControlPropertyAttribute
    {
        public OnValueChangedAttribute(string path ) : base(path)
        {
        }
        protected override void OnInit(ControlAttribute ctl)
        {
        }
        public void OnValueChange()
        {
            if(AssemblyUtil.SetMemberValue(ctl.Target, path, ctl.Value))
            {
            }
            else
            {
                AssemblyUtil.InvokeObjMethod(ctl.Target, path);
            }
        }
    }
}
