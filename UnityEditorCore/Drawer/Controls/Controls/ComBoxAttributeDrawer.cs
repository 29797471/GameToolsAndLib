using UnityEditor;
using UnityCore;
using System.Linq;
using System;
using UnityEngine;

namespace UnityEditorCore
{
    /// <summary>
    /// 绘制组合框
    /// </summary>
    [CustomPropertyDrawer(typeof(ComBoxAttribute))]
    public class ComBoxAttributeDrawer: CqPropertyDrawer<ComBoxAttribute>
    {
        public override Action OnCqGUI(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Enum:
                    {
                        if(attribute.DisplayMembers == null)
                        {
                            attribute.DisplayMembers = EnumUtil.GetEnumNames(attribute.InfoType);
                        }
                        switch (attribute.style)
                        {
                            case ComBoxStyle.RadioBox:
                                {
                                    var index = EditorGUI.Popup(GetDrawRect(), "", property.enumValueIndex, attribute.DisplayMembers);
                                    return () =>property.enumValueIndex = index;
                                }
                            case ComBoxStyle.CheckBox:
                                {
                                    var index = EditorGUI.MaskField(GetDrawRect(), "", property.intValue, attribute.DisplayMembers);
                                    return () =>property.intValue = index;
                                }
                        }
                    }
                    break;
                case SerializedPropertyType.Integer:
                    {
                        if (attribute.DisplayMembers == null && attribute.Items!=null)
                        {
                            var temp = new System.Collections.Generic.List<string>();
                            foreach (var it in attribute.Items)
                            {
                                if(attribute.displayMemberName==null)
                                {
                                    temp.Add(it.ToString());
                                }
                                else temp.Add(AssemblyUtil.GetMemberValue(it, attribute.displayMemberName).ToString());
                            }
                            attribute.DisplayMembers = temp.ToArray();
                        }
                        if (attribute.Items==null)
                        {
                            var index = EditorGUI.IntField(GetDrawRect(), property.intValue);
                            return () => property.intValue = index;
                        }
                        else
                        {
                            switch (attribute.style)
                            {
                                case ComBoxStyle.RadioBox:
                                    {
                                        var index = EditorGUI.Popup(GetDrawRect(), "", property.intValue, attribute.DisplayMembers);
                                        return () => property.intValue = index;
                                    }
                                case ComBoxStyle.CheckBox:
                                    {
                                        if(attribute.DisplayMembers.Length==0)
                                        {
                                            var index = EditorGUI.IntField(GetDrawRect(), property.intValue);
                                            return () => property.intValue = index;
                                        }
                                        else
                                        {
                                            var index = EditorGUI.MaskField(GetDrawRect(), "", property.intValue, attribute.DisplayMembers);
                                            return () => property.intValue = index;
                                        }
                                    }
                            }
                            return null;
                        }
                    }
                    //break;
                case SerializedPropertyType.String:
                    {
                        if (attribute.DisplayMembers == null)
                        {
                            var temp = new System.Collections.Generic.List<string>();
                            foreach (var it in attribute.Items)
                            {
                                temp.Add(it.ToString());
                            }
                            attribute.DisplayMembers = temp.ToArray();
                        }
                        var index = EditorGUI.Popup(GetDrawRect(), "",
                            attribute.Items.IndexOf(property.stringValue),
                            attribute.DisplayMembers);
                        return () => property.stringValue = attribute.DisplayMembers[index];
                    }
                   //break;
                default:
                    {
                        Debug.Log(property.propertyType);
                        break;
                    }
            }

            return null;
        }
    }
}
