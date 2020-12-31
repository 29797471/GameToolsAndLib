using System.Collections.Generic;
using UnityCore;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 继承PropertyDrawer, 必须放入Editor文件夹下
/// </summary>
[CustomPropertyDrawer(typeof(CqLabelAttribute))]
public class CqLabelAttributeDrawer : CqPropertyDrawer<CqLabelAttribute>
{
    public override System.Action OnCqGUI(SerializedProperty property)
    {
        switch (property.propertyType)
        {
            case SerializedPropertyType.Float:
                {
                    var value = EditorGUI.FloatField(GetDrawRect(), property.floatValue);
                    return () => property.floatValue = value;
                }
            case SerializedPropertyType.Integer:
                {
                    var value = EditorGUI.IntField(GetDrawRect(), property.intValue);
                    return () => property.intValue = value;
                }
            case SerializedPropertyType.Color:
                {
                    var value = EditorGUI.ColorField(GetDrawRect(), property.colorValue);
                    return () => property.colorValue = value;
                }
            case SerializedPropertyType.Vector2:
                {
                    var value = EditorGUI.Vector2Field(GetDrawRect(), "", property.vector2Value);
                    return () => property.vector2Value = value;
                }
            case SerializedPropertyType.Vector3:
                {
                    var value = EditorGUI.Vector3Field(GetDrawRect(), "", property.vector3Value);
                    return () => property.vector3Value = value;
                }
            case SerializedPropertyType.Vector4:
                {
                    var value = EditorGUI.Vector4Field(GetDrawRect(), "", property.vector4Value);
                    return () => property.vector4Value = value;
                }
            case SerializedPropertyType.Quaternion:
                {
                    var value = UnityUtil.Vector4ToQuaternion(EditorGUI.Vector4Field(GetDrawRect(), "", (UnityUtil.QuaternionToVector4(property.quaternionValue))));
                    return () => property.quaternionValue = value;
                }
            case SerializedPropertyType.Rect:
                {
                    var value = EditorGUI.RectField(GetDrawRect(), "", property.rectValue);
                    return () => property.rectValue = value;
                }
            case SerializedPropertyType.ObjectReference:
                {
                    var value = EditorGUI.ObjectField(GetDrawRect(), property.objectReferenceValue, fieldInfo.FieldType,true);
                    return () => property.objectReferenceValue = value;
                }
            case SerializedPropertyType.Generic:
                {
                    Debug.Log(property.name);
                    //var label = EditorGUI.BeginProperty(GetDrawRect(), new GUIContent(""), property);
                    //EditorGUI.EndProperty();
                    //var value = EditorGUI.ObjectField(GetDrawRect(), property.objectReferenceValue, fieldInfo.FieldType);
                    return null;
                }
            default:
                {
                    Debug.LogError("未处理"+property.propertyType);
                    return null;
                }
        }
    }
}
