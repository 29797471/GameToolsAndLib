using System;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 按钮控件
    /// </summary>
    public class ButtonAttribute : ControlAttribute
    {
        public string btnName;
        /// <summary>
        /// 按钮控件
        /// </summary>
        public ButtonAttribute(string btnName) : base(null)
        {
            this.btnName = btnName;
        }
        public Action Click;
        protected override void OnGUI()
        {
            base.OnGUI();
            if(GUILayout.Button(btnName))
            {
                if (Click != null) Click();
            }
        }
    }
}

