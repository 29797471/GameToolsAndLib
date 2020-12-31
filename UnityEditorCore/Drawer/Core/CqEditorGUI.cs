using UnityEngine;
using UnityEditor;
using UnityCore;

public class CqEditorGUI: PropertyDrawer
{
    //EditorGUIUtility.labelWidth??
    protected Rect _position;
    
    /// <summary>
    /// 刚开始绘制时position的x=0,y=绘制起点的列坐标 width宽度为列宽
    /// </summary>
    public void BeginProperty(Rect position, SerializedProperty property, GUIContent label)
    {
        label=EditorGUI.BeginProperty(position, label, property);
        _position = position;
        _position.y += EditorGUIConfig.Unity_Item_Y;
    }
    public void EndProperty()
    {
        NewLine();
        EditorGUI.EndProperty();
    }


    public void NewLine(float height = EditorGUIConfig.Unity_Item_Height)
    {
        _position.x = EditorGUIConfig.Unity_Item_X;
        _position.y += height;
    }

    /// <summary>
    /// 获取一个将要绘制的控件的区域(从编辑面板的最后向前定位)
    /// </summary>
    protected Rect DrawControlFromStart(float endPercent , float endOffset , float height)
    {
        return BaseDrawControl(0, EditorGUIConfig.Unity_Item_X, endPercent, endOffset,height);
    }

    /// <summary>
    /// 获取一个将要绘制的控件的区域(从编辑面板的最后向前定位)
    /// </summary>
    protected Rect DrawControlToEnd(float startPercent , float startOffset , float height )
    {
        return BaseDrawControl(startPercent, startOffset,1,0, height);
    }

    /// <summary>
    /// 获取一个将要绘制的控件的区域,基于起始描点的偏移和终止描点的偏移来定位区域
    /// </summary>
    protected Rect BaseDrawControl(float startPercent, float startOffset,float endPercent, float endOffset, float height )
    {
        var _startX = _position.width * startPercent + startOffset;
        _startX = Mathf.Clamp(_startX, 0, _position.width);
        var _endX = _position.width * endPercent + endOffset;
        _endX = Mathf.Clamp(_endX, 0, _position.width);
        return new Rect(_startX, _position.y, _endX - _startX, height);
    }
    
    public bool ChangeCheck(System.Action act)
    {
        EditorGUI.BeginChangeCheck();
        if (act != null) act();
        return EditorGUI.EndChangeCheck();
    }


    /// <summary>
    /// 折叠标签
    /// </summary>
    public bool FolderOut(bool value, string content)
    {
        return EditorGUI.Foldout(BaseDrawControl(0,0,1,0,20f), value, content);
    }

}