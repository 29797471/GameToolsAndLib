using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 文本框 支持多行文本
    /// 可编辑float,int,string
    /// </summary>
    public class TextBoxAttribute:ControlAttribute
    {
        public bool multiline;

        public TextBoxAttribute(string label,bool multiline = false):base(label)
        {
            this.multiline = multiline;
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            if (Value == null) Value = "";
            if (multiline)
            {
                var t=GUILayout.TextArea(Value.ToString(), options);
                if (t != Value.ToString())
                {
                    Value = t;
                }
            }
            else
            {
                var t = GUILayout.TextField(Value.ToString(), options);
                if (t != Value.ToString())
                {
                    Value = t;
                }
            }
        }
    }
}
