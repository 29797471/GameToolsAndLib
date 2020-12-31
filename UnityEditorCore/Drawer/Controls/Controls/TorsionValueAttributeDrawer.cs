using MVL;
using UnityCore;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 继承PropertyDrawer, 必须放入Editor文件夹下
/// </summary>
[CustomPropertyDrawer(typeof(TorsionValueAttribute))]
public class TorsionValueAttributeDrawer : CqPropertyDrawer<TorsionValueAttribute>
{
    
    float?      v_float;
    Vector2?    v_Vector2;
    Vector3?    v_Vector3;
    Vector4?    v_Vector4;
    Color?      v_Color;
    string lastV;
    public override System.Action OnCqGUI(SerializedProperty property)
    {
        if (lastV != property.stringValue)
        {
            lastV = property.stringValue;
            v_float = null;
            v_Vector2 = null;
            v_Vector3 = null;
            v_Color = null;
        }

        var style=(BindingTweenType)attribute.GetOtherMemberValue();
        switch (style)
        {
            case BindingTweenType.System_Single:
                {
                    
                    if (v_float == null)
                    {
                        try
                        {
                            v_float = float.Parse(property.stringValue);
                        }
                        catch (System.Exception)
                        {
                            v_float = default(float);
                            property.stringValue = v_float.ToString();
                        };
                    }
                    var value = EditorGUI.FloatField(GetDrawRect(), "", (float)v_float);
                    return () =>
                    {
                        v_float = value;
                        property.stringValue = v_float.ToString();
                    };
                }
            case BindingTweenType.UnityEngine_Vector2:
                {
                    if (v_Vector2 == null)
                    {
                        if (property.stringValue.IsNullOrEmpty())
                        {
                            v_Vector2 = default(Vector2);
                            property.stringValue = v_Vector2.ToString();
                        }
                        else
                        {
                            v_Vector2 = Torsion.TryDeserialize<Vector2>(property.stringValue);
                        }
                    }
                    var value = EditorGUI.Vector2Field(GetDrawRect(), "", (Vector2)v_Vector2);
                    return () =>
                    {
                        v_Vector2 = value;
                        property.stringValue = Torsion.Serialize(v_Vector2);
                    };
                }
            case BindingTweenType.UnityEngine_Vector3:
                {
                    if(v_Vector3 == null)
                    {
                        try
                        {
                            v_Vector3 = Torsion.Deserialize<Vector3>(property.stringValue);
                        }
                        catch (System.Exception)
                        {
                            v_Vector3 = default(Vector3);
                            property.stringValue = v_Vector3.ToString();
                        }
                    }
                    var value = EditorGUI.Vector3Field(GetDrawRect(), "", (Vector3)v_Vector3);
                    return () =>
                    {
                        v_Vector3 = value;
                        property.stringValue = Torsion.Serialize(v_Vector3);
                    };
                }

            case BindingTweenType.UnityEngine_Quaternion:
            case BindingTweenType.UnityEngine_Vector4:
                {
                    if (v_Vector4 == null)
                    {
                        try
                        {
                            v_Vector4 = Torsion.Deserialize<Vector4>(property.stringValue);
                        }
                        catch (System.Exception)
                        {
                            v_Vector4 = default(Vector4);
                            property.stringValue = v_Vector4.ToString();
                        }
                    }
                    var value = EditorGUI.Vector4Field(GetDrawRect(), "", (Vector4)v_Vector4);
                    return () =>
                    {
                        v_Vector4 = value;
                        property.stringValue = Torsion.Serialize(v_Vector4);
                    };
                }
            case BindingTweenType.UnityEngine_Color:
                {
                    if (v_Color == null)
                    {
                        try
                        {
                            v_Color = Torsion.Deserialize<Color>(property.stringValue);
                        }
                        catch (System.Exception)
                        {
                            v_Color = default(Color);
                            property.stringValue = v_Color.ToString();
                        }
                    }
                    var value = EditorGUI.ColorField(GetDrawRect(), "", (Color)v_Color);
                    return () =>
                    {
                        v_Color = value;
                        property.stringValue = Torsion.Serialize(v_Color);
                    };
                }
            default:
                {
                    return null;
                }
        }
    }
}

