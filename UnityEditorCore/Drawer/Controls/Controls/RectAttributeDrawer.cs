using UnityEditor;
using UnityCore;
using UnityEngine;

namespace UnityEditorCore
{
    /// <summary>
    /// 绘制区域
    /// </summary>
    [CustomPropertyDrawer(typeof(RectAttribute))]
    public class RectAttributeDrawer : CqPropertyDrawer<RectAttribute>
    {
        public override System.Action OnCqGUI(SerializedProperty property)
        {
            var value = EditorGUI.RectField(GetDrawRect(), property.rectValue);
            return () => property.rectValue = value;
        }
    }
}

