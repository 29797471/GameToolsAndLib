using UnityEditor;
using UnityCore;
using UnityEngine;

namespace UnityEditorCore
{
    /// <summary>
    /// 绘制勾选框
    /// </summary>
    [CustomPropertyDrawer(typeof(CheckBoxAttribute))]
    public class CheckBoxAttributeDrawer : CqPropertyDrawer<CheckBoxAttribute>
    {
        /// <summary>
        /// 当面板绘制时
        /// </summary>
        public override System.Action OnCqGUI(SerializedProperty property)
        {
            if (SerializedPropertyType.Boolean == property.propertyType)
            {
                var value = EditorGUI.Toggle(GetDrawRect(), property.boolValue);
                return () =>
                {
                    property.boolValue = value;
                };
            }
            return null;
        }
    }
}


