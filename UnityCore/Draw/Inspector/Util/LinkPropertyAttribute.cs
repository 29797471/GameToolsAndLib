using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 提供通过该特性来关联一个属性来代替本身的字段
    /// </summary>
    public class LinkPropertyAttribute : ControlPropertyAttribute
    {
        public readonly string propertyName;

        public LinkPropertyAttribute(string propertyName):base(propertyName)
        {
            this.propertyName = propertyName;
        }
        protected override void OnInit(ControlAttribute ctl)
        {
            base.OnInit(ctl);
            ctl.LinkPropertyName = propertyName;
        }
    }
}
