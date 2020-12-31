using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using CqCore;

/// <summary>
/// 修饰类中方法
/// 当在由类生成的控件上右键时弹出菜单会包含该项目
/// </summary>
public class MenuItemAttribute : MethodAttribute
{
    public string icon;
    public string path;
    public Key key;
    public ModifierKeys modifiers;
    public MenuItemAttribute(string path,string icon=null, Key key=0, ModifierKeys modifiers=0):base()
    {
        this.path = path;
        this.icon = icon;
        this.key = key;
        this.modifiers = modifiers;
    }

    /// <summary>
    /// 添加右键菜单
    /// </summary>
    public void AddContextMenu(FrameworkElement panel,Func <object[] > GetArgs=null)
    {
        var mi = new MenuItem();
        mi.Header = path;
        if (icon != null)
        {
            mi.Icon = ShellIcon.DrawImage(icon,25);
        }
        mi.Click += (obj, e) =>Exec(GetArgs == null ? null : GetArgs());
        if (panel.ContextMenu == null) panel.ContextMenu = new ContextMenu();
        var attr= AssemblyUtil.GetMemberAttribute<IsEnabledAttribute>(Info, false, Parent);
        if(attr!=null)
        {
            attr.Init(mi);
        }
        panel.ContextMenu.Items.Add(mi);

        //菜单栏快捷方式
        WinUtil.SetInputCommandX(panel, ()=>Exec(GetArgs==null?null:GetArgs()), key, modifiers);
    }
}

