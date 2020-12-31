using UnityEditor;
using UnityCore;
using UnityEditorInternal;
using UnityEngine;
using System.Collections;

namespace UnityEditorCore
{
    /// <summary>
    /// 绘制列表框
    /// </summary>
    [CustomPropertyDrawer(typeof(ListBoxAttribute))]
    public class ListBoxAttributeDrawer : CqPropertyDrawer<ListBoxAttribute>
    {
        private ReorderableList _list;

        private SerializedProperty GetListProperty(SerializedProperty property)
        {
            return property.serializedObject.FindProperty(fieldInfo.Name);
        }
        bool IsFristProperty(SerializedProperty property)
        {
            return (fieldInfo.Name + ".Array.data[0]" == property.propertyPath);
        }
        private ReorderableList GetReorderableList(SerializedProperty property)
        {
            if (_list == null)
            {
                //_list = new ReorderableList(a , a.GetType());
                var listProperty = GetListProperty(property);
                _list = new ReorderableList(property.serializedObject, listProperty, true, false, true, true);
                _list.drawHeaderCallback += delegate (Rect rect)
                {
                    EditorGUI.LabelField(rect, attribute.label);
                };
                var itemType = fieldInfo.FieldType.GetGenericArguments()[0];
                if (itemType.IsValueType || itemType==typeof(string) || itemType.IsSubclassOf(typeof(ScriptableObject)))
                {
                    _list.drawElementCallback = delegate (Rect rect, int index, bool isActive, bool isFocused)
                    {
                        EditorGUI.PropertyField(rect, listProperty.GetArrayElementAtIndex(index),true);
                    };
                }
                else
                {
                    _list.drawElementCallback = delegate (Rect rect, int index, bool isActive, bool isFocused)
                    {
                        EditorGUI.ObjectField(rect, listProperty.GetArrayElementAtIndex(index));
                    };
                }

                if(!itemType.IsValueType)
                {
                    //_list.onAddCallback = (list) =>
                    //{
                    //    var obj=AssemblyUtil.CreateInstance(itemType);

                    //    _list.list.Add(obj);
                    //};
                }
                _list.elementHeightCallback = (index) =>
                {
                    //return _list.elementHeight;
                    var it = listProperty.GetArrayElementAtIndex(index);
                    return it.isExpanded ? _list.elementHeight : 18f;
                };
                _list.onChangedCallback = (list) =>
                {
                    
                };
                
            }
            return _list;
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property)+ (IsFristProperty(property)?30f:0f);
            //if (!IsFristProperty(property)) return 0f;
            
            //var list = GetListProperty(property);
            //return property.isExpanded ? _list.elementHeight : 18f;
        }

        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!IsFristProperty(property)) return;
            var list = GetReorderableList(property);
            var listProperty = GetListProperty(property);
            var height = 0f;
            for (var i = 0; i < listProperty.arraySize; i++)
            {
                height = Mathf.Max(height, EditorGUI.GetPropertyHeight(listProperty.GetArrayElementAtIndex(i)));
            }
            list.elementHeight = height;
            list.footerHeight = 20f;
            //list.headerHeight = 50f;
            list.DoList(position);
        }
    }
}


