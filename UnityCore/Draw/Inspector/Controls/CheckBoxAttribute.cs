using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 勾选框
    /// 修饰 bool字段
    /// </summary>
    public class CheckBoxAttribute:ControlAttribute
    {
        /// <summary>
        /// 勾选框
        /// 修饰 bool字段
        /// </summary>
        public CheckBoxAttribute(string label) : base(label)
        {
        }
        protected override void OnGUI()
        {
            base.OnGUI();
            var v = (bool)Info.GetValue(Target);
            var bl = GUILayout.Toggle(v, "");
            if(bl!=v)
            {
                Info.SetValue(Target, bl);
            }
        }
    }
}
