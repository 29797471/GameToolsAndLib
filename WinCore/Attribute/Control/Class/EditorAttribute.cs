using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

/*
 * 对对象编辑大致分成4种:
1.在其它容器中(由外部来定义)
	a.在groupBox中
	b.在TabControl的分页中
	c.在listView中
    d.在listBox中
2.在弹出窗口中
	a.由自身成员来编辑
 */

/// <summary>
/// 定义弹窗的编辑样式
/// 当在成员中定义以点击方式编辑时,弹出窗口
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
public class EditorAttribute : Attribute
{
    public string name;
    public string icon;
    public Type customUserControl;
    /// <summary>
    /// 对象成员的排列方式
    /// 当只有一重优先级时按该方式排序
    /// 有两重时第2个用另一方式排序
    /// </summary>
    public Orientation ori= Orientation.Vertical;
    public EditorAttribute(string name ,string icon,System.Type customUserControl)
    {
        this.name = name;
        this.icon = icon;
        this.customUserControl = customUserControl;
    }

    /// <summary>
    /// 定义名称和子控件的排列方式
    /// </summary>
    public EditorAttribute(string name = null, string icon=null, Orientation ori= Orientation.Vertical)
    {
        this.name = name;
        this.ori = ori;
        this.icon = icon;
    }

    /// <summary>
    /// 将对象绘制在TabItem,GroupBox 等等
    /// </summary>
    public T DrawHCC<T>(object target)where T: HeaderedContentControl,new()
    {
        var hcc = new T();
        if (name == null) name = target.ToString();
        hcc.Header = name;

        var sv = new ScrollViewer()
        {
            HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto
        };
        sv.Content = DrawStackPanel(target);
        hcc.Content = sv;
        return hcc;
    }

    /// <summary>
    /// 将对象绘制在TabItem 等等
    /// </summary>
    public void DrawInTabItem(TabItem hcc, object target)
    {
        if (name == null) name = target.ToString();
        hcc.Header = name;

        var sv = new ScrollViewer()
        {
            HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto
        };
        sv.Content = DrawStackPanel(target);
        hcc.Content = sv;
    }

    /// <summary>
    /// 绘制对象返回一个面板,包含元素控件集
    /// </summary>
    public StackPanel DrawStackPanel(object target)
    {
        if(customUserControl!=null)
        {
            var sp = new StackPanel();
            var inst=(UIElement)AssemblyUtil.CreateInstance(customUserControl);
            sp.Children.Add(inst);
            sp.DataContext = target;
            return sp;
        }
        if (target == null) return null;
        var panel = WinUtil.DrawStackPanel(target, target.GetType().GetMembers().ToList(), ori);
        WinUtil.AddContextMenu(panel, target);
        panel.DataContext = target;
        return panel;
    }

    
}
