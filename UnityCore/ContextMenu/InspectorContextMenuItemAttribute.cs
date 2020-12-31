using System;

/// <summary>
/// 在自定义的编辑器窗口创建一个上下文菜单并在其中完成静态方法调用
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class InspectorContextMenuItemAttribute : Attribute
{
    public string menuItem;
    public int priority;

    public InspectorContextMenuItemAttribute(string menuItem, int priority)
    {
        this.menuItem = menuItem; 
        this.priority = priority;
    }

    public InspectorContextMenuItemAttribute(string menuItem) : this(menuItem, 0)
    {

    }
}