using UnityCore;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 继承PropertyDrawer, 必须放入Editor文件夹下
/// </summary>
[CustomPropertyDrawer(typeof(TextBoxAttribute))]
public class TextBoxAttributeDrawer : CqPropertyDrawer<TextBoxAttribute>
{
    public override System.Action OnCqGUI(SerializedProperty property)
    {
        if (property.hasMultipleDifferentValues)
        {
            var str = EditorGUI.TextField(GetDrawRect(), "--", EditorStyles.textField);
            switch (property.propertyType)
            {
                case SerializedPropertyType.String:
                    return ()=>property.stringValue = str;
                case SerializedPropertyType.Float:
                    return ()=>property.floatValue = float.Parse(str);
                case SerializedPropertyType.Integer:
                    return ()=>property.intValue = int.Parse(str);
            }
            return null;
        }
        else
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Float:
                    {
                        var newValue = EditorGUI.FloatField(GetDrawRect(), property.floatValue, EditorStyles.numberField);
                        return ()=>property.floatValue = newValue;
                    }
                case SerializedPropertyType.Integer:
                    {
                        var newValue = EditorGUI.IntField(GetDrawRect(), property.intValue);
                        return () => property.intValue = newValue;
                    }
                case SerializedPropertyType.String:
                    {
                        string stringValue = property.stringValue;
                        if (attribute.LinkValue != null)
                        {
                            stringValue = attribute.LinkValue.ToString();
                        }
                        string str = null;
                        if (attribute.multiline)
                        {
                            attribute.SetViewLineCount(stringValue.NumberOfLines());
                            str = EditorGUI.TextArea(GetDrawRect(), stringValue, EditorStyles.textArea);
                        }
                        else
                        {
                            str = EditorGUI.TextField(GetDrawRect(), stringValue, EditorStyles.textField);
                        }

                        return ()=>property.stringValue = str;
                    }
            }
        }
        return null;
        
        //EditorGUI.PropertyField(position, property, setProperty.Label==null?label:setProperty.Label);
        
    }
}
