using UnityEngine;
using UnityEditor;
using System;
using UnityCore;

/// <summary>
/// 继承PropertyDrawer, 必须放入Editor文件夹下
/// </summary>
[CustomPropertyDrawer(typeof(VectorAttribute))]
public class VectorAttributeDrawer : CqPropertyDrawer<VectorAttribute>
{
    public override Action OnCqGUI(SerializedProperty property)
    {
        switch (property.propertyType)
        {
            case SerializedPropertyType.Vector2:
                {
                    var value = Vector2Field(property.vector2Value);
                    return () => property.vector2Value = value;
                }
            case SerializedPropertyType.Vector3:
                {
                    //var bl=EditorGUILayout.PropertyField(property);
                    //return null;
                    var value = Vector3Field(property.vector3Value);
                    return () => property.vector3Value = value;
                }
            case SerializedPropertyType.Vector4:
                {
                    var value = Vector4Field(property.vector4Value);
                    return () => property.vector4Value = value;
                }
        }
        return null;
    }
    public Vector2 Vector2Field(Vector2 value)
    {
        return EditorGUI.Vector2Field(GetDrawRect(), "", value);
    }
    public Vector3 Vector3Field(Vector3 value)
    {
        return EditorGUI.Vector3Field(GetDrawRect(), "", value);
    }
    public Vector4 Vector4Field(Vector4 value)
    {
        //var rect = GetDrawRect(); rect.y -= EditorGUIConfig.Unity_Vector4_deltaY;
        return EditorGUI.Vector4Field(GetDrawRect(), "", value);
    }
}