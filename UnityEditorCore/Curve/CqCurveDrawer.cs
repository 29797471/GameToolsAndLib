using UnityCore;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CqCurve))]
public class CqCurveDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label.text = "3D曲线";
        var bl = EditorGUI.PropertyField(position, property, label, true);
    }
}
