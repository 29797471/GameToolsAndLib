using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

/// <summary>
/// 定义该类在右键菜单中的路径
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class MenuItemPathAttribute : Attribute
{
    public string icon;
    public string path;
    public Key key;
    public ModifierKeys modifiers;

    public MenuItemPathAttribute(string path, string icon = null, Key key = 0, ModifierKeys modifiers = 0):base()
    {
        this.path = path;
        this.icon = icon;
        this.key = key;
        this.modifiers = modifiers;
    }

    /// <summary>
    /// 给控件添加子菜单
    /// </summary>
    public void AddMenuItem(FrameworkElement fe,Action OnClick,object ToolTip = null)
    {
        var paths = path.Split('/');
        if (fe.ContextMenu == null) fe.ContextMenu = new ContextMenu();
        var Items = fe.ContextMenu.Items;
        MenuItem find_menu = null;
        foreach (var header in paths)
        {
            find_menu = null;
            foreach (MenuItem item in Items)
            {
                if (item.Header.ToString() == header)
                {
                    find_menu = item;
                    break;
                }
            }
            if (find_menu == null)
            {
                find_menu = new MenuItem();
                find_menu.Header = header;
                find_menu.ToolTip = ToolTip;
                Items.Add(find_menu);
            }
            Items = find_menu.Items;
        }
        find_menu.Click += (obj, e) => OnClick?.Invoke();
        WinUtil.SetInputCommandX(fe, OnClick,key, modifiers);
        //var attr = AssemblyUtil.GetAttribute<ToolTipAttribute>(type);
        //if (attr != null) menu.ToolTip = attr.value;
    }
}