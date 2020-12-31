using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityCore
{
    /// <summary>
    /// 滑动条
    /// 编辑一个范围属性
    /// </summary>
    public class SliderAttribute: ControlAttribute
    {
        public float min;
        public float max;

        /// <summary>
        /// 滑动条
        /// </summary>
        public SliderAttribute(string name, float min=float.MinValue, float max=float.MaxValue) : base(name)
        {
            this.min = min;
            this.max = max;
        }
    }
}
