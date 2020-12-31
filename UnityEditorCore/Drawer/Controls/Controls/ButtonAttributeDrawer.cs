using UnityEditor;
using UnityCore;
using UnityEngine;

namespace UnityEditorCore
{
    /// <summary>
    /// 绘制列表框
    /// </summary>
    [CustomPropertyDrawer(typeof(ButtonAttribute))]
    public class ButtonAttributeDrawer : CqPropertyDrawer<ButtonAttribute>
    {
        /// <summary>
        /// 当面板绘制时
        /// </summary>
        public override System.Action OnCqGUI(SerializedProperty property)
        {
            GUI.Button(BaseDrawControl(0.5f, -50, 0.5f, 50, 20f), attribute.btnName);
            return () =>
            {
                if (attribute.Click != null) attribute.Click();
            };
        }
    }
}

