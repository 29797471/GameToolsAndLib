using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 颜色编辑框
    /// </summary>
    public class ColorAttribute : ControlAttribute
    {
        public ColorAttribute(string label):base(label)
        {
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            
        }
    }
}
