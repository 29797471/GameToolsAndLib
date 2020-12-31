using System.Collections.Generic;
using UnityCore;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 继承PropertyDrawer, 必须放入Editor文件夹下
/// </summary>
[CustomPropertyDrawer(typeof(ObjectLabelAttribute))]
public class ObjectLabelAttributeDrawer : CqPropertyDrawer<ObjectLabelAttribute>
{
    static Dictionary<System.Type, SerializedPropertyType> dic;
    public override System.Action OnCqGUI(SerializedProperty property)
    {
        var propertyType = property.propertyType;
        if (attribute.GetPropertyType != null)
        {
            if (dic == null)
            {
                dic = new Dictionary<System.Type, SerializedPropertyType>();
                dic[typeof(int)] = SerializedPropertyType.Integer;
                dic[typeof(float)] = SerializedPropertyType.Float;
                dic[typeof(Vector2)] = SerializedPropertyType.Vector2;
                dic[typeof(Vector3)] = SerializedPropertyType.Vector3;
                dic[typeof(Vector4)] = SerializedPropertyType.Vector4;
                dic[typeof(Quaternion)] = SerializedPropertyType.Quaternion;
                dic[typeof(Rect)] = SerializedPropertyType.Rect;
                dic[typeof(Color)] = SerializedPropertyType.Color;
            }
            var type = attribute.GetPropertyType();
            dic.TryGetValue(type, out propertyType);
            Debug.LogError("dic:" + propertyType);
        }
        Debug.LogError(propertyType);
        switch (propertyType)
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
                    Debug.LogError("a");
                    var value = EditorGUI.Vector3Field(GetDrawRect(), "", property.vector3Value);
                    Debug.LogError("b");
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
                    var value = EditorGUI.ObjectField(GetDrawRect(), property.objectReferenceValue, fieldInfo.FieldType, true);
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
                    Debug.LogError("未处理" + property.propertyType);
                    return null;
                }
        }
    }
}

