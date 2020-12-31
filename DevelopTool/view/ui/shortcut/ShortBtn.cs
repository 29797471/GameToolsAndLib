using System.Windows;

/// <summary>
/// 快捷方式按钮
/// </summary>
[Editor, HorizontalAlignment(HorizontalAlignment.Center)]
public class ShortBtn : NotifyObject
{
    [Priority(0)]
    [Image, Width(32), Height(32)]
    public string ShortcutPath
    {
        get { return mShortcutPath; }
        set { mShortcutPath = value; Update("ShortcutPath"); }
    }
    public string mShortcutPath;

    [Priority(1)]
    [Label, Visibility("IsEditing", AttributeTarget.Parent, "x=0")]
    public string StaticName
    {
        get
        {
            return Name;
        }
    }

    [Priority(2)]
    [TextBox, Visibility("IsEditing", AttributeTarget.Parent)]
    public string Name
    {
        get { if (mName == null) mName = "temp"; return mName; }
        set { mName = value; Update("Name"); Update("StaticName"); }
    }
    public string mName;

    public bool IsEditing
    {
        get
        {
            return mIsEditing;
        }

        set
        {
            mIsEditing = value;
            Update("IsEditing");
        }
    }

    private bool mIsEditing;

    public bool IsFilePath
    {
        get
        {
            return FileOpr.IsFilePath(ShortcutPath);
        }
    }

}