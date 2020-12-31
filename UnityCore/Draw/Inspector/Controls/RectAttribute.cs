using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 区域
    /// </summary>
    public class RectAttribute : ControlAttribute
    {
        /// <summary>
        /// 按钮控件
        /// </summary>
        public RectAttribute(string label) : base(label)
        {
        }
        protected override void OnGUI()
        {
            base.OnGUI();
        }
    }
}

