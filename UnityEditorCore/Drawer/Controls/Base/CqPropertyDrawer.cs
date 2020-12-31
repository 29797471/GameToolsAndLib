using UnityCore;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 当在编辑面板上右键弹出菜单时,OnGUI不会执行，关闭菜单后才会触发
/// </summary>
/// <typeparam name="T"></typeparam>
public class CqPropertyDrawer<T> : CqEditorGUI where T : ControlAttribute
{
    public new T attribute { get { return base.attribute as T; } }

    protected void BaseOnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);
    }
    protected float BaseGetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);

        if (attribute.Visible)
        {
            BeginProperty(position, property, label);

            EditorGUI.BeginDisabledGroup(!attribute.Enabled);
            EditorGUI.BeginChangeCheck();
            DrawPerfix(property);
            var ChangeValue = OnCqGUI(property);

            if (EditorGUI.EndChangeCheck())
            {
                if (ChangeValue != null)
                {
                    ChangeValue();
                    property.serializedObject.ApplyModifiedProperties();
                    //attribute.IsDirty = true;
                    attribute.OnValueChanged();
                    //Event.PopEvent(new Event() { type = EventType.Repaint });
                }
            }
            EditorGUI.EndDisabledGroup();
            //var rightMouseClick = Event.current.type == EventType.ContextClick && position.Contains(Event.current.mousePosition);
            EndProperty();
        }
    }

    bool hasInit;
    public  override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        //高度是最优先调用的,所以放在此处初始化
        if (!hasInit)
        {
            if(!property.propertyPath.Contains("."))
            {
                attribute.SetTarget(fieldInfo, property.serializedObject.targetObject);
            }
            else
            {
                var propertyPath = property.propertyPath.ReplaceAll(".Array.data", "");
                //Debug.Log("property.propertyPath= " + propertyPath);
                var index=propertyPath.LastIndexOf('.');
                var path = propertyPath.Remove(index);
                //Debug.Log("path= "+ path);
                var target = AssemblyUtil.GetMemberValueByExpression(property.serializedObject.targetObject, path);
                if(target!=null)attribute.SetTarget(fieldInfo, target);
                else
                {
                    Debug.LogError("target==null path = " + propertyPath);
                }
            }
            
            hasInit = true;
        }
        if (!attribute.Visible) return 0f;
        return attribute.RealHeight+ EditorGUIConfig.Unity_Item_Y;
    }

    /// <summary>
    /// 绘制前缀
    /// </summary>
    public void DrawPerfix( SerializedProperty property)
    {
        {
            var guiContent = new GUIContent(attribute.label, attribute.LabelToolTip);
            //var n = StringUtil.GetChineseCharCount(attribute.label);
            //var contentWidth = n * 15 + (attribute.label.Length - n) * 9;
            var contentWidth = GUIStyle.none.CalcSize(guiContent).x;
            if (attribute.realPrefixWidth < contentWidth) attribute.realPrefixWidth = contentWidth;
            EditorGUI.PrefixLabel(BaseDrawControl(0, EditorGUIConfig.Unity_Item_X, 0, attribute.realPrefixWidth, EditorGUIConfig.Unity_Item_Height), guiContent);
        }
    }
    public virtual System.Action OnCqGUI(SerializedProperty property)
    {
        return null;
    }
    /// <summary>
    /// 返回实际绘制大小
    /// </summary>
    protected Rect GetDrawRect()
    {
        return BaseDrawControl(0, attribute.realPrefixWidth+30, 1, 0, attribute.RealHeight);
    }
    
}