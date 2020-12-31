using UnityCore;
using UnityEditor;
/// <summary>
/// 继承PropertyDrawer, 必须放入Editor文件夹下
/// </summary>
[CustomPropertyDrawer(typeof(ColorAttribute))]
public class ColorAttributeDrawer : CqPropertyDrawer<ColorAttribute>
{
    public override System.Action OnCqGUI(SerializedProperty property)
    {
        if (property.hasMultipleDifferentValues)
        {
            
            return null;
        }
        else
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Color:
                    {
                        var newValue = EditorGUI.ColorField(GetDrawRect(), property.colorValue);
                        return () => property.colorValue = newValue;
                    }
            }
        }
        return null;

        //EditorGUI.PropertyField(position, property, setProperty.Label==null?label:setProperty.Label);

    }
}

