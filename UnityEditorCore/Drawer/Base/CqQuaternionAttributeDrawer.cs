using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using System;
using UnityCore;

/// <summary>
/// 继承PropertyDrawer, 必须放入Editor文件夹下
/// </summary>
[CustomPropertyDrawer(typeof(CqQuaternionAttribute))]
public class CqQuaternionAttributeDrawer : CqPropertyDrawer<CqQuaternionAttribute>
{
    [MenuItem("CONTEXT/Quaternion/=本地角度(localRotation)")]
    static void SetTolocalRotation(MenuCommand inCommand)
    {
        OnSetTolocalRotation();
    }
    static Action OnSetTolocalRotation;
    public override System.Action OnCqGUI(SerializedProperty property)
    {
        if (GUI.Button(DrawControlFromStart(0,100,20f), attribute.label))
        {
            var cmd = new MenuCommand(property.serializedObject.targetObject);
            OnSetTolocalRotation = () =>
            {
                property.quaternionValue = (property.serializedObject.targetObject as Component).transform.localRotation;
                property.serializedObject.ApplyModifiedProperties();
            };
            EditorUtility.DisplayPopupMenu(new Rect(Event.current.mousePosition, Vector2.zero), "CONTEXT/Quaternion/", cmd);
        }
        var value = UnityUtil.Vector4ToQuaternion(EditorGUI.Vector4Field(GetDrawRect(), "",(UnityUtil.QuaternionToVector4(property.quaternionValue))));
        return () => property.quaternionValue = value;
    }

}