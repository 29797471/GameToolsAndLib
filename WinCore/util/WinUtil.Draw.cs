using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using WinCore;

public static partial class WinUtil
{
    
    /// <summary>
    /// 按优先级分组,从低到高排列,纵横交错组织控件
    /// </summary>
    public static StackPanel DrawStackPanel(object target, List<MemberInfo> list, Orientation ori = Orientation.Horizontal, int layer = 0)
    {
        var panel = new StackPanel();
        
        if ( ori == Orientation.Horizontal)
        {
            panel.Orientation = Orientation.Horizontal;
            panel.Margin = new Thickness(0, 5, 0, 5);
        }
        else
        {
            panel.Orientation = Orientation.Vertical;
            panel.Margin = new Thickness(5, 0, 5, 0);
        }
        var attrs = AssemblyUtil.GetClsssAttributes(target);
        foreach (var attr in attrs)
        {
            if (attr is WidthAttribute || attr is HeightAttribute) continue;
            if (attr is LinkControlPropertyAttribute)
            {
                var it = attr as LinkControlPropertyAttribute;
                it.Init(panel, null);
            }
            else if (attr is LinkControlMemberAttribute)
            {
                var it = attr as LinkControlMemberAttribute;
                it.Init(panel);
            }
        }
        var xx = list.GroupBy(x =>
        {
            var p = AssemblyUtil.GetMemberAttribute<PriorityAttribute>(x);
            if (p != null && layer < p.prioritys.Length) return p.prioritys[layer];
            else return 0;
        }).ToList();
        if (xx.Count == 1)
        {
            foreach (var member in xx.First())
            {
                //绘制对象成员
                var attr = AssemblyUtil.GetMemberAttribute<ControlPropertyAttribute>(member, false, target);
                if (attr != null) attr.AddControl(panel);
            }
        }
        else
        {
            xx.Sort((x, y) => x.Key - y.Key);
            foreach (var it in xx)
            {
                panel.Children.Add(DrawStackPanel(target, it.ToList(), 1 - ori, layer + 1));
            }
        }

        return panel;
    }

    /// <summary>
    /// 添加对象成员定义的菜单项
    /// </summary>
    public static void AddContextMenu(FrameworkElement root, object target,Func<object[]> GetArgs=null)
    {
        if (target == null) return;
        if(root.ContextMenu!=null) root.ContextMenu.Items.Clear();
        //root.ContextMenu = new ContextMenu();
        var list = PriorityAttribute.GetMembersBySort(target.GetType());
        foreach (var member in list)
        {
            var attr = AssemblyUtil.GetMemberAttribute<MenuItemAttribute>(member,false,target);
            if (attr != null) attr.AddContextMenu(root,GetArgs);
        }
    }


    /// <summary>
    /// 将对象绘制在Panel中
    /// </summary>
    public static Panel DrawPanel(object target)
    {
        if (target == null) return null;
        var ma = AssemblyUtil.GetMemberAttribute<EditorAttribute>(target.GetType());
        if(ma != null)return ma.DrawStackPanel(target);
        return null;
    }

    /// <summary>
    /// 将对象绘制在GroupBox,TabItem等等
    /// </summary>
    public static StackPanel DrawStackPanel(object target)
    {
        if (target == null) return null;
        return AssemblyUtil.GetMemberAttribute<EditorAttribute>(target.GetType()).DrawStackPanel(target);
    }

    /// <summary>
    /// 将对象绘制在GroupBox,TabItem等等
    /// </summary>
    public static T DrawHCC<T>(object target)where T: HeaderedContentControl,new()
    {
        if (target == null) return null;
        return AssemblyUtil.GetClassAttribute<EditorAttribute>(target).DrawHCC<T>(target);
    }

    /// <summary>
    /// 绘制被Editor特性修饰的对象
    /// 在弹出窗口中绘制对象
    /// 确定时返回编辑的结果,否则返回null
    /// </summary>
    public static T OpenEditorWindow<T>(T target,bool clone=true,bool keepInArea=false,bool xEqualCancel=true) where T :class
    {
        var targetType = target.GetType();
        var attr = AssemblyUtil.GetClassAttribute<EditorAttribute>(target);
        var win = new EditorWindow();
        //{
        //    var rect = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
        //    win.Top = (rect.Height - win.Height) / 2;
        //    win.Left = (rect.Width - win.Width) / 2;
        //}
        if (clone)win.Target = Torsion.Clone(target);
        else win.Target = target;
        var attrs = AssemblyUtil.GetClsssAttributes(target);
        //ControlAttribute.SetBindings(win, attrs);
        
        foreach (var att in attrs)
        {
            if (att is LinkControlMemberAttribute)
            {
                var it = att as LinkControlMemberAttribute;
                it.Init(win);
            }
            else if(att is EditorAttribute)
            {
                var it = att as EditorAttribute;
                win.Title = it.name;
            }
        }
        
        //保证在工作区域内
        if (keepInArea)
        {
            win.Loaded += (obj, e) =>
            {
                var rect = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;

                //if (win.Top < 0 || win.Top + win.Height > rect.Height)
                {
                    win.Top = (rect.Height - win.Height) / 2;
                }
                //if (win.Left < 0 || win.Left + win.Width > rect.Width)
                {
                    win.Left = (rect.Width - win.Width) / 2;
                }
            };
        }
        var bl =(bool)win.ShowDialog();//x和cancel都是false
        if ( win.onBtn)
        {
            return bl?(T)win.Target:null;
        }
        
        return xEqualCancel?null:(T)win.Target;
    }


    /// <summary>
    /// 异步弹出窗口绘制对象
    /// </summary>
    public static void OpenEditorWindowAsync<T>(T target, bool clone = true) where T : class
    {
        var targetType = target.GetType();
        var attr = AssemblyUtil.GetClassAttribute<EditorAttribute>(target);
        var win = new EditorWindow();
        if (clone) win.Target = Torsion.Clone(target);
        else win.Target = target;
        var attrs = AssemblyUtil.GetClsssAttributes(target);
        //ControlAttribute.SetBindings(win, attrs);

        foreach (var att in attrs)
        {
            if (att is LinkControlMemberAttribute)
            {
                var it = att as LinkControlMemberAttribute;
                it.Init(win);
            }
        }
        win.Show();
    }

}
