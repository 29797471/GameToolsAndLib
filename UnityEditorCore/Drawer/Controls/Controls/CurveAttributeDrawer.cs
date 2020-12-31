using UnityCore;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 继承PropertyDrawer, 必须放入Editor文件夹下
/// </summary>
[CustomPropertyDrawer(typeof(CurveAttribute))]
public class CurveAttributeDrawer : CqPropertyDrawer<CurveAttribute>
{
    public override System.Action OnCqGUI(SerializedProperty property)
    {
        if (SerializedPropertyType.AnimationCurve == property.propertyType)
        {
            var value = EditorGUI.CurveField(GetDrawRect(), property.animationCurveValue);
            NewLine(20);
            var rect = BaseDrawControl(0, EditorGUIConfig.Unity_Item_X, 0, 70, EditorGUIConfig.Unity_Item_Height);
            if (GUI.Button(rect, "常用"))
            {
                CqEaseWindow.GetCurve(curve =>
                {
                    if (curve != null)
                    {
                        property.animationCurveValue = curve;
                        property.serializedObject.ApplyModifiedProperties();
                    }
                });
            }
            return () => property.animationCurveValue = value;
        }
        return null;
    }
}