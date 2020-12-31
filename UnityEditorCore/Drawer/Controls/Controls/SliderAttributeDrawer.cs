using UnityCore;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 继承PropertyDrawer, 必须放入Editor文件夹下
/// </summary>
[CustomPropertyDrawer(typeof(SliderAttribute))]
public class SliderAttributeDrawer : CqPropertyDrawer<SliderAttribute>
{
    public override System.Action OnCqGUI(SerializedProperty property)
    {
        if (property.hasMultipleDifferentValues)
        {
            var str = EditorGUI.TextField(GetDrawRect(), "--", EditorStyles.textField);
            switch (property.propertyType)
            {
                case SerializedPropertyType.Float:
                    {
                        var value = float.Parse(str);
                        return () => property.floatValue = value;
                    }
                case SerializedPropertyType.Integer:
                    {
                        var value = int.Parse(str);
                        return () => property.intValue = value;
                    }
            }
        }

        switch (property.propertyType)
        {
            case SerializedPropertyType.Float:
                {
                    var value = Slider(property.floatValue, attribute.min, attribute.max);
                    return () => property.floatValue = value;
                }
            case SerializedPropertyType.Integer:
                {
                    var value = (int)Slider(property.intValue, attribute.min, attribute.max);
                    return () => property.intValue = value;
                }
        }
        return null;
    }
    public float Slider(float value, float leftValue, float rightValue)
    {
        return EditorGUI.Slider(GetDrawRect(), value, leftValue, rightValue);
    }
}