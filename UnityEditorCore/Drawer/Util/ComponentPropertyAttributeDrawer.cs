using UnityEngine;
using UnityEditor;
using UnityCore;
using System.Reflection;

/// <summary>
/// 继承PropertyDrawer, 必须放入Editor文件夹下
/// </summary>
[CustomPropertyDrawer(typeof(ComponentPropertyAttribute))]
public class ComponentPropertyAttributeDrawer : CqPropertyDrawer<ComponentPropertyAttribute>
{
    void OnEditorEnd(SerializedProperty property, Component comp, string propertyName, MemberTypes memberType)
    {
        var com = new ComponentProperty() { name = propertyName, com = comp};
        AssemblyUtil.SetMemberValue(property.serializedObject.targetObject, property.propertyPath, com);
        //property.serializedObject.ApplyModifiedProperties();
        //Event.PopEvent(new Event() { type = EventType.Repaint });
        EditorUtility.SetDirty(property.serializedObject.targetObject);

        property.serializedObject.ApplyModifiedProperties();
        //property.serializedObject.UpdateIfRequiredOrScript();
    }
    public override System.Action OnCqGUI(SerializedProperty property)
    {
        ComponentProperty compro = AssemblyUtil.GetMemberValue(property.serializedObject.targetObject, property.propertyPath) as ComponentProperty;

        //EditorGUI.LabelField(GetDrawRect(), compro.ToString());
        //关联组件不是该对象的组件时清除掉
        if (compro != null && compro.com != null)
        {
            if (attribute.SrcObj == null)
            {
                var obj = property.serializedObject.targetObject as MonoBehaviour;
                if (obj != null && obj.gameObject != compro.com.gameObject) compro.com = null;
            }
            else
            {
                if (attribute.SrcObj != compro.com.gameObject) compro.com = null;
            }
        }
        var xx = attribute.SrcObj;
        if (xx == null)
        {
            xx = (property.serializedObject.targetObject as Component).gameObject;
        }
        if (compro != null)
        {
            if(GUI.Button(GetDrawRect(), compro.ToString(), "DropDown"))
            {
                ComponentPropertyWindow.Instance.EditObject(xx, (comp, propertyName, memberType) =>
                {
                    OnEditorEnd(property, comp, propertyName, memberType);
                    dirty = true;
                }, new System.Type[] { attribute.GetPropertyType() }, MemberTypes.Field | MemberTypes.Property);
            }
        }
        
        if (dirty)
        {
            dirty = false;
            attribute.OnValueChanged();
        }
        return null;
    }
    bool dirty;
}