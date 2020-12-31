using CqCore;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using WinCore;

public enum DoubleClickStyle
{
    Default,
    /// <summary>
    /// 在打开的窗口中编辑
    /// </summary>
    OpenEditorWindow,
}
/// <summary>
/// 链接一个双击的回调
/// </summary>
public class DoubleSelectedValueAttribute : LinkControlPropertyAttribute
{
    public string methodName;
    public DoubleClickStyle style;
    public DoubleSelectedValueAttribute(string methodName) : base(null)
    {
        this.methodName = methodName;
    }
    public DoubleSelectedValueAttribute(DoubleClickStyle style = DoubleClickStyle.OpenEditorWindow) : base(null)
    {
        this.style = style;
    }
    protected override void OnInit(FrameworkElement fe)
    {
        var ctl = fe as Control;
        if (ctl == null) return;
        switch (style)
        {
            case DoubleClickStyle.Default:
                {
                    ctl.MouseDoubleClick += (obj, args) =>
                    {
                        //双击时是选中列表框背景时返回
                        if (args.OriginalSource is ScrollViewer) return;
                        GongSolutions.Wpf.DragDrop.DragDrop.CancelDrag();
                        var selectItem = (ctl as Selector).SelectedItem;
                        if (ctl is Selector && selectItem != null)
                        {
                            AssemblyUtil.InvokeMethod(Parent, methodName, selectItem);
                        }
                    };
                }
                break;
            case DoubleClickStyle.OpenEditorWindow:
                {
                    ctl.MouseDoubleClick += (obj, args) =>
                    {
                        GongSolutions.Wpf.DragDrop.DragDrop.CancelDrag();
                        var s = ctl as Selector;
                        if (s == null) return;
                        var select =s.SelectedItem;
                        if (select != null)
                        {
                            select = WinUtil.OpenEditorWindow(select);
                            if (select != null) (Target as IList)[s.SelectedIndex] = select;
                        }
                        
                    };
                }
                break;
        }
    }
}
